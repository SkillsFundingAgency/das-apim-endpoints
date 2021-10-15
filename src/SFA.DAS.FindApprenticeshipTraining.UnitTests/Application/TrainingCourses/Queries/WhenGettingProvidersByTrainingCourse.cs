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
            int shortlistItemCount,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            GetTrainingCourseProvidersQueryHandler handler)
        {
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
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            mockLocationLookup
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync((LocationItem) null);
            
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers);
            result.Total.Should().Be(apiResponse.TotalResults);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
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
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            query.Location = $"{locationName}, {authorityName} ";
            query.Lat = 0;
            query.Lon = 0;

            mockLocationLookupService
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
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
    }
}