using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Azure;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Services;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingProviderByTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_The_Standard_And_The_Provider_For_That_Course_And_Shortlist_Count_From_Course_Delivery_Api_Client(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apprenticeFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ProviderStandard.Should().BeEquivalentTo(apiProviderDetailsResponse, x => x.ExcludingMissingMembers());
            result.ProviderStandard.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeFeedbackResponse);
            result.Course.Should().BeEquivalentTo(apiCourseResponse);
            result.ShortlistItemCount.Should().Be(shortlistItems.Count);
            mockRoatpV2ApiClient.Verify(x => x.Get<GetProviderDetailsForCourse>(It.IsAny<GetProviderByCourseAndUkprnRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_ShortlistUserId_Then_Sets_ShortlistCount_To_Zero(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            query.ShortlistUserId = null;
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apprenticeFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShortlistItemCount.Should().Be(0);

            mockShortlistApiClient.Verify(x => x.Get<List<ShortlistItem>>(It.Is<GetShortlistForUserRequest>(r => r.ShortlistUserId == query.ShortlistUserId)), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_If_NoShortlistItems_Found_Then_Sets_ShortlistId_To_Null_And_ShortlistCount_To_Zero(
           GetTrainingCourseProviderQuery query,
           GetProviderDetailsForCourse apiProviderDetailsResponse,
           GetStandardsListItem apiCourseResponse,
           GetOverallAchievementRateResponse apiAchievementRateResponse,
           List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
           GetTotalProvidersForStandardResponse ukprnsCountResponse,
           GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
           [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
           [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
           [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
           [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
           GetTrainingCourseProviderQueryHandler handler)
        {
            List<ShortlistItem> shortlistItems = new();
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apprenticeFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShortlistItemCount.Should().Be(shortlistItems.Count);
            result.ProviderStandard.ShortlistId.Should().BeNull();
            mockShortlistApiClient.Verify(x => x.Get<List<ShortlistItem>>(It.Is<GetShortlistForUserRequest>(r => r.ShortlistUserId == query.ShortlistUserId)));
        }

        [Test, MoqAutoData]
        public async Task Then_If_No_Matching_ShortlistItems_Then_Sets_ShortlistId_To_Null_And_ShortlistCount_To_Zero(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apprenticeFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShortlistItemCount.Should().Be(shortlistItems.Count);
            result.ProviderStandard.ShortlistId.Should().BeNull();
            mockShortlistApiClient.Verify(x => x.Get<List<ShortlistItem>>(It.Is<GetShortlistForUserRequest>(r => r.ShortlistUserId == query.ShortlistUserId)));
        }

        [Test, MoqAutoData]
        public async Task Then_If_Location_In_Query_And_Matching_ShortlistItem_Found_Then_Set_ShortlistId(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<ILocationLookupService> mockLookupService,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            mockLookupService
                .Setup(l => l.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync(new LocationItem (query.Location, new[] { query.Lat, query.Lon }, string.Empty));
            var matchingItem = new ShortlistItem
            {
                Id = Guid.NewGuid(),
                Ukprn = query.ProviderId,
                Larscode = query.CourseId,
                Latitude = query.Lat,
                Longitude = query.Lon
            };
            shortlistItems.Add(matchingItem);
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apprenticeFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShortlistItemCount.Should().Be(shortlistItems.Count);
            result.ProviderStandard.ShortlistId.Should().Be(matchingItem.Id);
            mockShortlistApiClient.Verify(x => x.Get<List<ShortlistItem>>(It.Is<GetShortlistForUserRequest>(r => r.ShortlistUserId == query.ShortlistUserId)));
        }

        [Test, MoqAutoData]
        public async Task Then_If_Location_Not_In_Query_And_Matching_ShortlistItem_Found_Then_Set_ShortlistId(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<ILocationLookupService> mockLookupService,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            query.Location = null;
            query.Lat = default;
            query.Lon = default;
            mockLookupService
                .Setup(l => l.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync(() => null);
            var matchingItem = new ShortlistItem
            {
                Id = Guid.NewGuid(),
                Ukprn = query.ProviderId,
                Larscode = query.CourseId
            };
            shortlistItems.Add(matchingItem);
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apprenticeFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.ShortlistItemCount.Should().Be(shortlistItems.Count);
            result.ProviderStandard.ShortlistId.Should().Be(matchingItem.Id);
            mockShortlistApiClient.Verify(x => x.Get<List<ShortlistItem>>(It.Is<GetShortlistForUserRequest>(r => r.ShortlistUserId == query.ShortlistUserId)));
        }

        [Test, MoqAutoData]
        public async Task Then_Gets_The_Overall_Achievement_Rate_Data_From_The_Course_SubjectSectorArea(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            query.Lat = 0;
            query.Lon = 0;
            query.Location = string.Empty;
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apprenticeFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.OverallAchievementRates.Should().BeEquivalentTo(apiAchievementRateResponse.OverallAchievementRates);
        }

        [Test, MoqAutoData]
        public async Task Then_Does_Not_Return_Additional_Courses_If_No_Additional_Courses(
            GetTrainingCourseProviderQuery query,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            mockShortlistApiClient
                .Setup(x => x.Get<List<ShortlistItem>>(It.Is<GetShortlistForUserRequest>(r => r.ShortlistUserId == query.ShortlistUserId)))
                .ReturnsAsync(shortlistItems);
            mockRoatpV2ApiClient
                .Setup(client => client.Get<List<GetProviderAdditionalStandardsItem>>(It.Is<GetProviderAdditionalStandardsRequest>(c =>
                    c.GetUrl.Contains(query.ProviderId.ToString()
                    ))))
                .ReturnsAsync(new List<GetProviderAdditionalStandardsItem>());
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockRoatpV2ApiClient.Setup(client => client.Get<GetOverallAchievementRateResponse>(It.Is<GetOverallAchievementRateRequest>(c =>
                    c.GetUrl.Contains(apiCourseResponse.SectorSubjectAreaTier2Description)
                )))
                .ReturnsAsync(apiAchievementRateResponse);
            mockRoatpV2ApiClient
                .Setup(x => x.Get<GetTotalProvidersForStandardResponse>(
                    It.Is<GetTotalProvidersForStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString()))))
                .ReturnsAsync(ukprnsCountResponse);
            apprenticeFeedbackResponse.Ukprn = query.ProviderId;
            mockApprenticeFeedbackApiClient
                        .Setup(s => s.GetWithResponseCode<GetApprenticeFeedbackResponse>(It.IsAny<GetApprenticeFeedbackDetailsRequest>()))
                        .ReturnsAsync(new ApiResponse<GetApprenticeFeedbackResponse>(apprenticeFeedbackResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result.AdditionalCourses.Should().BeEquivalentTo(new List<GetAdditionalCourseListItem>());
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Additional_Courses_for_Provider_In_Alphabetical_Order(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse appfeedbackResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, appfeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            var filteredProvidersStandardResponse = allProviderStandardsResponse
                .Where(x => x.IsApprovedByRegulator != false || string.IsNullOrEmpty(x.ApprovalBody)).ToList();
            result.AdditionalCourses.Should().BeInAscendingOrder(c => c.Title);
            result.AdditionalCourses.Should().BeEquivalentTo(filteredProvidersStandardResponse, options =>
                options
                    .Excluding(c => c.IsApprovedByRegulator)
                    .Excluding(c => c.ApprovalBody)
                    .WithMapping<GetAdditionalCourseListItem>(c => c.LarsCode, s => s.Id)
                    .WithMapping<GetAdditionalCourseListItem>(c => c.CourseName, s => s.Title)
                    );
        }

        [Test, MoqAutoData]
        public async Task Then_Returns_Totals_For_Standard(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetApprenticeFeedbackResponse appfeedbackResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, appfeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.TotalProviders.Should().Be(ukprnsCountResponse.ProvidersCount);
            result.TotalProvidersAtLocation.Should().Be(ukprnsCountResponse.ProvidersCount);
        }

        [Test, MoqAutoData]
        public async Task Then_Additional_Courses_for_Provider_Should_Contain_Course_Passed_To_Handler(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            List<GetStandardsListItem> allStandards,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apiFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            query.ShortlistUserId = null;
            apiCourseResponse.LarsCode = query.CourseId;
            allStandards.Add(new GetStandardsListItem
            {
                LarsCode = apiCourseResponse.LarsCode,
                Title = apiCourseResponse.Title,
                Level = apiCourseResponse.Level
            });
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apiFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var filteredProvidersStandardResponse = allProviderStandardsResponse
                .Where(x => x.IsApprovedByRegulator != false || string.IsNullOrEmpty(x.ApprovalBody)).ToList();

            var result = await handler.Handle(query, CancellationToken.None);

            result.AdditionalCourses.Should().BeEquivalentTo(filteredProvidersStandardResponse, options => 
                options
                    .Excluding(c => c.IsApprovedByRegulator)
                    .Excluding(c => c.ApprovalBody)
                    .WithMapping<GetAdditionalCourseListItem>(c => c.LarsCode, s => s.Id)
                    .WithMapping<GetAdditionalCourseListItem>(c => c.CourseName, s => s.Title)
                    );
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_Lat_Lon_Data_The_Location_Is_Used(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            LocationItem locationLookupResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            List<ShortlistItem> shortlistItems,
            GetApprenticeFeedbackResponse apiFeedbackResponse,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apiFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            mockLocationLookupService
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync(locationLookupResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Location.Should().BeEquivalentTo(locationLookupResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_Location_In_The_Request_The_Location_Data_Is_Added_To_The_Get_Course_Provider_Request(
            string locationName,
            string authorityName,
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            LocationItem locationLookupResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetApprenticeFeedbackResponse apiFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            query.Location = $"{locationName}, {authorityName} ";
            query.Lat = 0;
            query.Lon = 0;
            mockLocationLookupService
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync(locationLookupResponse);

            mockRoatpV2ApiClient
                .Setup(c => c.Get<GetProviderDetailsForCourse>(It.IsAny<GetProviderByCourseAndUkprnRequest>()))
                .ReturnsAsync(apiProviderDetailsResponse);

            ArrangeClients(query, apiProviderDetailsResponse, apiCourseResponse, apiAchievementRateResponse, allProviderStandardsResponse, ukprnsCountResponse, apiFeedbackResponse, mockCoursesApiClient, mockApprenticeFeedbackApiClient, mockRoatpV2ApiClient, shortlistItems, mockShortlistApiClient);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Location.Should().BeEquivalentTo(locationLookupResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Provider_Is_Not_Found_For_Location_And_Without_Location_Null_Returned(
            string locationName,
            string authorityName,
            GetTrainingCourseProviderQuery query,
            GetStandardsListItem apiCourseResponse,
            GetStandardsListResponse allCoursesApiResponse,
            LocationItem locationLookupResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            List<ShortlistItem> shortlistItems,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ILocationLookupService> mockLocationLookupService,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseProviderQueryHandler handler)
        {
            mockShortlistApiClient
                .Setup(x => x.Get<List<ShortlistItem>>(It.Is<GetShortlistForUserRequest>(r => r.ShortlistUserId == query.ShortlistUserId)))
                .ReturnsAsync(shortlistItems);
            query.Location = $"{locationName}, {authorityName} ";
            mockLocationLookupService
                .Setup(service => service.GetLocationInformation(query.Location, query.Lat, query.Lon, false))
                .ReturnsAsync(locationLookupResponse);
            mockRoatpV2ApiClient
                .Setup(x => x.Get<GetTotalProvidersForStandardResponse>(
                    It.Is<GetTotalProvidersForStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString()))))
                .ReturnsAsync(ukprnsCountResponse);
            mockRoatpV2ApiClient
                .Setup(client => client.Get<List<GetProviderAdditionalStandardsItem>>(It.Is<GetProviderAdditionalStandardsRequest>(
                    c =>
                        c.GetUrl.Contains(query.ProviderId.ToString()
                        ))))
                .ReturnsAsync(allCoursesApiResponse.Standards.Select(s => new GetProviderAdditionalStandardsItem { LarsCode = s.LarsCode }).ToList());
            mockCoursesApiClient
                .Setup(client =>
                    client.Get<GetStandardsListItem>(
                        It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString()))))
                .ReturnsAsync(apiCourseResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetAvailableToStartStandardsListRequest>()))
                .ReturnsAsync(allCoursesApiResponse);
            mockRoatpV2ApiClient.Setup(client => client.Get<GetOverallAchievementRateResponse>(It.Is<GetOverallAchievementRateRequest>(
                    c =>
                        c.GetUrl.Contains(apiCourseResponse.SectorSubjectAreaTier2Description)
                )))
                .ReturnsAsync(apiAchievementRateResponse);
            apprenticeFeedbackResponse.Ukprn = query.ProviderId;
            mockApprenticeFeedbackApiClient
                .Setup(s => s.GetWithResponseCode<GetApprenticeFeedbackResponse>(It.IsAny<GetApprenticeFeedbackDetailsRequest>()))
                .ReturnsAsync(new ApiResponse<GetApprenticeFeedbackResponse>(apprenticeFeedbackResponse, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result.ProviderStandard.Should().BeNull();
            mockRoatpV2ApiClient.Verify(r => r.Get<GetProviderDetailsForCourse>(It.IsAny<GetProviderByCourseAndUkprnRequest>()));
        }

        private static void ArrangeClients(
            GetTrainingCourseProviderQuery query,
            GetProviderDetailsForCourse apiProviderCourseDetailsResponse,
            GetStandardsListItem apiCourseResponse,
            GetOverallAchievementRateResponse apiAchievementRateResponse,
            List<GetProviderAdditionalStandardsItem> allProviderStandardsResponse,
            GetTotalProvidersForStandardResponse ukprnsCountResponse,
            GetApprenticeFeedbackResponse apprenticeFeedbackResponse,
            Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackClient,
            Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            List<ShortlistItem> shortlistItems,
            Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient)
        {
            mockShortlistApiClient
                .Setup(x => x.Get<List<ShortlistItem>>(It.Is<GetShortlistForUserRequest>(r => r.ShortlistUserId == query.ShortlistUserId)))
                .ReturnsAsync(shortlistItems);

            mockRoatpV2ApiClient
                .Setup(client => client.Get<GetProviderDetailsForCourse>(It.Is<GetProviderByCourseAndUkprnRequest>(c =>
                    c.GetUrl.Contains(query.CourseId.ToString())
                    && c.GetUrl.Contains(query.ProviderId.ToString())
                    )))
                .ReturnsAsync(apiProviderCourseDetailsResponse);

            mockRoatpV2ApiClient
                .Setup(client => client.Get<List<GetProviderAdditionalStandardsItem>>(It.Is<GetProviderAdditionalStandardsRequest>(
                    c =>
                        c.GetUrl.Contains(query.ProviderId.ToString()
                        ))))
                .ReturnsAsync(allProviderStandardsResponse);

            mockRoatpV2ApiClient
                .Setup(x => x.Get<GetTotalProvidersForStandardResponse>(
                    It.Is<GetTotalProvidersForStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString()))))
                .ReturnsAsync(ukprnsCountResponse);

            mockCoursesApiClient
                .Setup(client =>
                    client.Get<GetStandardsListItem>(
                        It.Is<GetStandardRequest>(c => c.GetUrl.Contains(query.CourseId.ToString()))))
                .ReturnsAsync(apiCourseResponse);

            mockRoatpV2ApiClient.Setup(client => client.Get<GetOverallAchievementRateResponse>(It.Is<GetOverallAchievementRateRequest>(
                    c =>
                        c.GetUrl.Contains(apiCourseResponse.SectorSubjectAreaTier2Description)
                )))
                .ReturnsAsync(apiAchievementRateResponse);

            apprenticeFeedbackResponse.Ukprn = query.ProviderId;
            mockApprenticeFeedbackClient
                .Setup(s => s.GetWithResponseCode<GetApprenticeFeedbackResponse>(It.IsAny<GetApprenticeFeedbackDetailsRequest>()))
                .ReturnsAsync(new ApiResponse<GetApprenticeFeedbackResponse>(apprenticeFeedbackResponse, HttpStatusCode.OK, string.Empty));

        }
    }
}