using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.FindApprenticeshipTraining.Configuration;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api_With_Totals_And_Shortlist_Count(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetTotalProvidersForStandardResponse getTotalProvidersForStandardResponse,
            int shortlistItemCount,
            LocationItem locationItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            GetTrainingCourseQueryHandler handler)
        {
            //Arrange
            locationLookupService.Setup(x => x.GetLocationInformation(query.LocationName, query.Lat, query.Lon,false))
                .ReturnsAsync(locationItem);
            levelsApiResponse.Levels.First().Name = "GCSE";
            coursesApiResponse.Level = levelsApiResponse.Levels.First().Code;
            coursesApiResponse.LevelEquivalent = levelsApiResponse.Levels
                .Single(x => x.Code == coursesApiResponse.Level).Name;
            var url = new GetTotalProvidersForStandardRequest(query.Id).GetUrl;
            var shortlistUrl = new GetShortlistUserItemCountRequest((Guid)query.ShortlistUserId).GetUrl;

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync(coursesApiResponse);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);

            mockRoatpCourseManagementApiClient
                .Setup(client =>
                    client.Get<GetTotalProvidersForStandardResponse>(
                        It.Is<GetTotalProvidersForStandardRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(getTotalProvidersForStandardResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);
            mockShortlistApiClient.Setup(client=>client.Get<int>(It.Is<GetShortlistUserItemCountRequest>(
                c=>c.GetUrl.Equals(shortlistUrl)))).ReturnsAsync(shortlistItemCount);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE");
            result.ProvidersCount.Should().Be(getTotalProvidersForStandardResponse.ProvidersCount);
            result.ProvidersCountAtLocation.Should().Be(getTotalProvidersForStandardResponse.ProvidersCount);
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Location_Then_Location_Not_Passed(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetTotalProvidersForStandardResponse getTotalProvidersForStandardResponse,
            int shortlistItemCount,
            LocationItem locationItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> mockRoatpCourseManagementApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            GetTrainingCourseQueryHandler handler)
        {
            //Arrange
            locationLookupService
                .Setup(x => x.GetLocationInformation(query.LocationName, query.Lat, query.Lon,false))
                .ReturnsAsync((LocationItem)null);
            levelsApiResponse.Levels.First().Name = "GCSE";
            coursesApiResponse.Level = levelsApiResponse.Levels.First().Code;
            coursesApiResponse.LevelEquivalent = levelsApiResponse.Levels
                .Single(x => x.Code == coursesApiResponse.Level).Name;
            var url = new GetTotalProvidersForStandardRequest(query.Id).GetUrl;
            var shortlistUrl = new GetShortlistUserItemCountRequest((Guid)query.ShortlistUserId).GetUrl;

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync(coursesApiResponse);

            mockShortlistApiClient.Setup(client => client.Get<int>(It.Is<GetShortlistUserItemCountRequest>(
                c => c.GetUrl.Equals(shortlistUrl)))).ReturnsAsync(shortlistItemCount);

            mockRoatpCourseManagementApiClient
                .Setup(client =>
                    client.Get<GetTotalProvidersForStandardResponse>(
                        It.Is<GetTotalProvidersForStandardRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(getTotalProvidersForStandardResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE"); 
            result.ProvidersCount.Should().Be(getTotalProvidersForStandardResponse.ProvidersCount);
            result.ProvidersCountAtLocation.Should().Be(getTotalProvidersForStandardResponse.ProvidersCount);
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_No_Standard_Found_Returns_Empty_Response(
            GetTrainingCourseQuery query,
            GetLevelsListResponse levelsApiResponse,
            GetUkprnsForStandardAndLocationResponse courseDirectoryApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
            GetTrainingCourseQueryHandler handler)
        {

            //Arrange
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync((GetStandardsListItem) null);


            var url = new GetUkprnsForStandardAndLocationRequest(query.Id, query.Lat, query.Lon).GetUrl;
            mockCourseDeliveryApiClient
                .Setup(client =>
                    client.Get<GetUkprnsForStandardAndLocationResponse>(
                        It.Is<GetUkprnsForStandardAndLocationRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(courseDirectoryApiResponse);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeNull();
        }
    }
}