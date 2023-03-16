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
using System;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingProvidersByTrainingCourseAndAddingShortlistDetails
    {
        [Test,MoqAutoData]
        public async Task Then_Gets_The_Matched_ShortlistId_When_Ukprns_Locations_Larscodes_Match(
            GetTrainingCourseProvidersQuery query,
            GetStandardsListItem apiCourseResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            var shortlistId = Guid.NewGuid();
            var ukprn1 = 11111111;
            var ukprn2 = 11111111;
            var larscode1 = 1;
            var larscode2 = 1;
            var location1 = "loc 1";
            var location2 = "loc 1";

            var apiResponse = new GetProvidersListFromCourseIdResponse();
            var shortlist = new List<ShortlistItem>();
            var providers = new List<GetProvidersListItem>();

            var result = await ProcessTrainingCourseProvider(query, apiCourseResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockLocationLookup, mockRoatpApiClient, mockShortlistApiClient, handler, providers, ukprn1, apiResponse, location1, larscode1, shortlist, ukprn2, location2, shortlistId, larscode2);

            result.ShortlistItemCount.Should().Be(shortlist.ToList().Count);
            result.Providers.First().ShortlistId.Should().Be(shortlistId);
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_The_UnMatched_ShortlistId_When_Ukprns_Do_Not_Match(
            GetTrainingCourseProvidersQuery query,
            GetStandardsListItem apiCourseResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            var shortlistId = Guid.NewGuid();
            var ukprn1 = 11111111;
            var ukprn2 = 11111112;
            var larscode1 = 1;
            var larscode2 = 1;
            var location1 = "loc 1";
            var location2 = "loc 1";

            var apiResponse = new GetProvidersListFromCourseIdResponse();
            var shortlist = new List<ShortlistItem>();
            var providers = new List<GetProvidersListItem>();

            var result = await ProcessTrainingCourseProvider(query, apiCourseResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockLocationLookup, mockRoatpApiClient, mockShortlistApiClient, handler, providers, ukprn1, apiResponse, location1, larscode1, shortlist, ukprn2, location2, shortlistId, larscode2);

            result.ShortlistItemCount.Should().Be(shortlist.ToList().Count);
            result.Providers.First().ShortlistId.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_The_UnMatched_ShortlistId_When_LarsCodes_Do_Not_Match(
            GetTrainingCourseProvidersQuery query,
            GetStandardsListItem apiCourseResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            var shortlistId = Guid.NewGuid();
            var ukprn1 = 11111111;
            var ukprn2 = 11111111;
            var larscode1 = 1;
            var larscode2 = 2;
            var location1 = "loc 1";
            var location2 = "loc 1";

            var apiResponse = new GetProvidersListFromCourseIdResponse();
            var shortlist = new List<ShortlistItem>();
            var providers = new List<GetProvidersListItem>();

            var result = await ProcessTrainingCourseProvider(query, apiCourseResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockLocationLookup, mockRoatpApiClient, mockShortlistApiClient, handler, providers, ukprn1, apiResponse, location1, larscode1, shortlist, ukprn2, location2, shortlistId, larscode2);

            result.ShortlistItemCount.Should().Be(shortlist.ToList().Count);
            result.Providers.First().ShortlistId.Should().BeNull();
        }


        [Test, MoqAutoData]
        public async Task Then_Gets_The_UnMatched_ShortlistId_When_LocationDescriptions_Do_Not_Match(
            GetTrainingCourseProvidersQuery query,
            GetStandardsListItem apiCourseResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookup,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            GetTrainingCourseProvidersQueryHandler handler)
        {
            var shortlistId = Guid.NewGuid();
            var ukprn1 = 11111111;
            var ukprn2 = 11111111;
            var larscode1 = 1;
            var larscode2 = 1;
            var location1 = "loc 1";
            var location2 = "loc 2";

            var apiResponse = new GetProvidersListFromCourseIdResponse();
            var shortlist = new List<ShortlistItem>();
            var providers = new List<GetProvidersListItem>();

            var result = await ProcessTrainingCourseProvider(query, apiCourseResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockLocationLookup, mockRoatpApiClient, mockShortlistApiClient, handler, providers, ukprn1, apiResponse, location1, larscode1, shortlist, ukprn2, location2, shortlistId, larscode2);

            result.ShortlistItemCount.Should().Be(shortlist.ToList().Count);
            result.Providers.First().ShortlistId.Should().BeNull();
        }

        private static async Task<GetTrainingCourseProvidersResult> ProcessTrainingCourseProvider(GetTrainingCourseProvidersQuery query,
            GetStandardsListItem apiCourseResponse, Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient, Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            Mock<ILocationLookupService> mockLocationLookup, Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpApiClient, Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            GetTrainingCourseProvidersQueryHandler handler, List<GetProvidersListItem> providers, int ukprn1,
            GetProvidersListFromCourseIdResponse apiResponse, string location1, int larscode1, List<ShortlistItem> shortlist, int ukprn2,
            string location2, Guid shortlistId, int larscode2)
        {
            providers.Add(new GetProvidersListItem
            {
                Ukprn = ukprn1
            });


            apiResponse.Providers = providers;

            apiCourseResponse.Level = 1;
            query.Location = location1;
            query.Lat = 0;
            query.Lon = 0;
            query.Id = larscode1;

            shortlist.Add(new ShortlistItem
            {
                Ukprn = ukprn2,
                LocationDescription = location2,
                Id = shortlistId,
                Larscode = larscode2
            });

            foreach (var provider in apiResponse.Providers)
            {
                provider.IsApprovedByRegulator = null;
            }

            var providerUkprns = apiResponse.Providers.Select(s => s.Ukprn);


            mockRoatpApiClient
                .Setup(client => client.Get<GetProvidersListFromCourseIdResponse>(It.Is<GetProvidersByCourseIdRequest>(
                        c => c.GetUrl.Contains(query.Id.ToString())
                             && c.GetUrl.Contains($"api/courses/{query.Id}/providers")
                    )
                ))
                .ReturnsAsync(apiResponse);

            mockCoursesApiClient
                .Setup(client =>
                    client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiCourseResponse);

            mockShortlistApiClient
                .Setup(client =>
                    client.GetAll<ShortlistItem>(
                        It.Is<GetShortlistForUserIdRequest>(c => c.ShortlistUserId == query.ShortlistUserId)))
                .ReturnsAsync(shortlist);

            mockLocationLookup
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync((LocationItem)null);
            mockApprenticeFeedbackApiClient
                .Setup(s => s.GetAll<GetApprenticeFeedbackSummaryItem>(It.IsAny<GetApprenticeFeedbackSummaryRequest>()))
                .ReturnsAsync(new List<GetApprenticeFeedbackSummaryItem>());

            var result = await handler.Handle(query, CancellationToken.None);
            return result;
        }
    }
}