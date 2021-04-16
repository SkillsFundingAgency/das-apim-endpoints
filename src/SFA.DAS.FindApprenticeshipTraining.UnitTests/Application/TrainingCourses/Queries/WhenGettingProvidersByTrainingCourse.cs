using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingProvidersByTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_Standard_And_The_List_Of_Providers_For_That_Course_From_Course_Delivery_Api_Client_With_No_Location_And_ShortlistItem_Count(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            GetShowEmployerDemandResponse showEmployerDemandResponse,
            int shortlistItemCount,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingConfiguration>> config,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            config.Object.Value.EmployerDemandFeatureToggle = true;
            apiCourseResponse.Level = 1;
            query.Location = "";
            query.Lat = 0;
            query.Lon = 0;
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString()) 
                    && c.GetUrl.Contains($"sectorSubjectArea={apiCourseResponse.SectorSubjectAreaTier2Description}&level={apiCourseResponse.Level}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockEmployerDemandApiClient
                .Setup(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            mockLocationLookup
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon))
                .ReturnsAsync((LocationItem) null);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.ShowEmployerDemand.Should().BeTrue();
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
            result.Location.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
            string locationName,
            string authorityName,
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            LocationItem locationServiceResponse,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingConfiguration>> config,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            config.Object.Value.EmployerDemandFeatureToggle = true;
            query.Location = $"{locationName}, {authorityName} ";
            query.Lat = 0;
            query.Lon = 0;

            mockLocationLookupService
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon))
                .ReturnsAsync(locationServiceResponse);
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains(locationServiceResponse.GeoPoint.First().ToString())
                    && c.GetUrl.Contains(locationServiceResponse.GeoPoint.Last().ToString())
                    && c.GetUrl.Contains($"&sortOrder={query.SortOrder}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Name.Should().Be(locationServiceResponse.Name);
            result.Location.GeoPoint.Should().BeEquivalentTo(locationServiceResponse.GeoPoint);
        }

        [Test, MoqAutoData]
        public async Task And_ShowDemandResponse_Not_OK_Then_ShowDemand_False(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            int shortlistItemCount,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingConfiguration>> config,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            config.Object.Value.EmployerDemandFeatureToggle = true;
            apiCourseResponse.Level = 1;
            query.Location = "";
            query.Lat = 0;
            query.Lon = 0;
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString()) 
                    && c.GetUrl.Contains($"sectorSubjectArea={apiCourseResponse.SectorSubjectAreaTier2Description}&level={apiCourseResponse.Level}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockEmployerDemandApiClient
                .Setup(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()))
                .ReturnsAsync(HttpStatusCode.Forbidden);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            
            var result = await handler.Handle(query, CancellationToken.None);
            
            result.ShowEmployerDemand.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task And_If_Feature_Not_Enabled_For_Employer_Demand_Then_Show_Demand_False(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            int shortlistItemCount,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingConfiguration>> config,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            config.Object.Value.EmployerDemandFeatureToggle = false;
            apiCourseResponse.Level = 1;
            query.Location = "";
            query.Lat = 0;
            query.Lon = 0;
            mockApiClient
                .Setup(client => client.Get<GetProvidersListResponse>(It.Is<GetProvidersByCourseRequest>(c=>
                    c.GetUrl.Contains(query.Id.ToString()) 
                    && c.GetUrl.Contains($"sectorSubjectArea={apiCourseResponse.SectorSubjectAreaTier2Description}&level={apiCourseResponse.Level}")
                )))
                .ReturnsAsync(apiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockEmployerDemandApiClient
                .Setup(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()))
                .ReturnsAsync(HttpStatusCode.Forbidden);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            
            var result = await handler.Handle(query, CancellationToken.None);
            
            result.ShowEmployerDemand.Should().BeFalse();
            mockEmployerDemandApiClient
                .Verify(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()), Times.Never);
        }
        
    }
}