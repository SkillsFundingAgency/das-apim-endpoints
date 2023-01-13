using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Configuration;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingProvidersByTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_Standard_And_The_List_Of_Providers_For_That_Course_From_Course_Delivery_Api_Client_With_No_Location_And_ShortlistItem_Count(
            GetTrainingCourseProvidersQuery query,
            GetProvidersListFromCourseIdResponse apiResponse,
            GetStandardsListItem apiCourseResponse,
            IEnumerable<GetApprenticeFeedbackSummaryItem> apprenticeFeedbackResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<IRoatpV2ApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            IEnumerable<ShortlistItem> shortlist,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            apiCourseResponse.Level = 1;
            query.Location = "";
            query.Lat = 0;
            query.Lon = 0;

            var providerUkprns = apiResponse.Providers.Select(s => s.Ukprn);
            for (var i = 0; i < apprenticeFeedbackResponse.Count(); i++)
            {
                apprenticeFeedbackResponse.ToArray()[i].Ukprn = providerUkprns.ToArray()[i];
            }

            mockRoatpApiClient
                .Setup(client => client.Get<GetProvidersListFromCourseIdResponse>(It.Is<GetProvidersByCourseIdRequest>(
                    c => c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains($"api/courses/{query.Id}/providers")
                    )
                ))
                .ReturnsAsync(apiResponse);

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);

            mockShortlistApiClient
                .Setup(client => client.GetAll<ShortlistItem>(It.Is<GetShortlistForUserIdRequest>(c=>c.ShortlistUserId==query.ShortlistUserId)))
                    .ReturnsAsync(shortlist);

            mockLocationLookup
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync((LocationItem)null);
            mockApprenticeFeedbackApiClient
                 .Setup(s => s.GetAll<GetApprenticeFeedbackSummaryItem>(It.IsAny<GetApprenticeFeedbackSummaryRequest>()))
                 .ReturnsAsync(apprenticeFeedbackResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers, options => options.Excluding(s => s.ApprenticeFeedback));
            result.Total.Should().Be(apiResponse.Providers.Count());
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.ShortlistItemCount.Should().Be(shortlist.ToList().Count);
            result.Location.Should().BeNull();

            foreach (var provider in result.Providers)
            {
                provider.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeFeedbackResponse.First(s => s.Ukprn == provider.Ukprn));
            }
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_A_Location_Supplied_It_Is_Searched_And_Passed_To_The_Provider_Search(
            string locationName,
            string authorityName,
            GetTrainingCourseProvidersQuery query,
            GetProvidersListFromCourseIdResponse apiResponse,
            IEnumerable<GetApprenticeFeedbackSummaryItem> apprenticeFeedbackResponse,
            GetStandardsListItem apiCourseResponse,
            LocationItem locationServiceResponse,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IRoatpV2ApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            query.Location = $"{locationName}, {authorityName} ";
            query.Lat = 0;
            query.Lon = 0;

            var providerUkprns = apiResponse.Providers.Select(s => s.Ukprn);
            for (var i = 0; i < apprenticeFeedbackResponse.Count(); i++)
            {
                apprenticeFeedbackResponse.ToArray()[i].Ukprn = providerUkprns.ToArray()[i];
            }

            mockLocationLookupService
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync(locationServiceResponse);
           
            mockRoatpApiClient
                .Setup(client => client.Get<GetProvidersListFromCourseIdResponse>(It.Is<GetProvidersByCourseIdRequest>(
                        c => c.GetUrl.Contains(query.Id.ToString())
                             && c.GetUrl.Contains($"api/courses/{query.Id}/providers")
                    )
                ))
                .ReturnsAsync(apiResponse);

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockApprenticeFeedbackApiClient
             .Setup(s => s.GetAll<GetApprenticeFeedbackSummaryItem>(It.IsAny<GetApprenticeFeedbackSummaryRequest>()))
             .ReturnsAsync(apprenticeFeedbackResponse);
            var result = await handler.Handle(query, CancellationToken.None);

            result.Providers.Should().BeEquivalentTo(apiResponse.Providers, options => options.Excluding(s => s.ApprenticeFeedback));
            result.Total.Should().Be(apiResponse.Providers.Count());
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.Location.Name.Should().Be(locationServiceResponse.Name);
            result.Location.GeoPoint.Should().BeEquivalentTo(locationServiceResponse.GeoPoint);

            foreach (var provider in result.Providers)
            {
                provider.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeFeedbackResponse.First(s => s.Ukprn == provider.Ukprn));
            }
        }
    }
}