using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api_With_Totals_And_Shortlist_Count(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetTotalProvidersForStandardResponse providersCountResponse,
            int shortlistItemCount,
            LocationItem locationItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IRoatpV2ApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseQueryHandler handler)
        {
            //Arrange
            locationLookupService.Setup(x => x.GetLocationInformation(query.LocationName, query.Lat, query.Lon, false))
                .ReturnsAsync(locationItem);
            levelsApiResponse.Levels.First().Name = "GCSE";
            coursesApiResponse.Level = levelsApiResponse.Levels.First().Code;
            coursesApiResponse.LevelEquivalent = levelsApiResponse.Levels
                .Single(x => x.Code == coursesApiResponse.Level).Name;
            var url = new GetTotalProvidersForStandardRequest(query.Id).GetUrl;

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync(coursesApiResponse);
            mockShortlistApiClient
                .Setup(client=>client.Get<int>(It.Is<GetShortlistUserItemCountRequest>(c => c.GetUrl.Contains($"api/Shortlist/users/{query.ShortlistUserId}/count"))))
                .ReturnsAsync(shortlistItemCount);
            mockRoatpV2ApiClient
                .Setup(client =>
                    client.Get<GetTotalProvidersForStandardResponse>(
                        It.Is<GetTotalProvidersForStandardRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(providersCountResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE");
            result.ProvidersCount.Should().Be(providersCountResponse.ProvidersCount);
            result.ProvidersCountAtLocation.Should().Be(providersCountResponse.ProvidersCount);
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Location_Then_Location_Not_Passed(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetTotalProvidersForStandardResponse providersCountApiResponse,
            int shortlistItemCount,
            LocationItem locationItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IShortlistApiClient<ShortlistApiConfiguration>> mockShortlistApiClient,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IRoatpV2ApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseQueryHandler handler)
        {
            //Arrange
            locationLookupService
                .Setup(x => x.GetLocationInformation(query.LocationName, query.Lat, query.Lon, false))
                .ReturnsAsync((LocationItem)null);
            levelsApiResponse.Levels.First().Name = "GCSE";
            coursesApiResponse.Level = levelsApiResponse.Levels.First().Code;
            coursesApiResponse.LevelEquivalent = levelsApiResponse.Levels
                .Single(x => x.Code == coursesApiResponse.Level).Name;
            var url = new GetTotalProvidersForStandardRequest(query.Id).GetUrl;

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync(coursesApiResponse);
            mockShortlistApiClient
                .Setup(client => client.Get<int>(It.Is<GetShortlistUserItemCountRequest>(c => c.GetUrl.Contains($"api/Shortlist/users/{query.ShortlistUserId}/count"))))
                .ReturnsAsync(shortlistItemCount);
            mockRoatpV2ApiClient
                .Setup(client =>
                    client.Get<GetTotalProvidersForStandardResponse>(
                        It.Is<GetTotalProvidersForStandardRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(providersCountApiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE");
            result.ProvidersCount.Should().Be(providersCountApiResponse.ProvidersCount);
            result.ProvidersCountAtLocation.Should().Be(providersCountApiResponse.ProvidersCount);
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_No_Standard_Found_Returns_Empty_Response(
            GetTrainingCourseQuery query,
            GetLevelsListResponse levelsApiResponse,
            GetTotalProvidersForStandardResponse courseDirectoryApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<IRoatpV2ApiClient<RoatpV2ApiConfiguration>> mockRoatpV2ApiClient,
            GetTrainingCourseQueryHandler handler)
        {

            //Arrange
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync((GetStandardsListItem) null);


            var url = new GetTotalProvidersForStandardRequest(query.Id).GetUrl;
            mockRoatpV2ApiClient
                .Setup(client =>
                    client.Get<GetTotalProvidersForStandardResponse>(
                        It.Is<GetTotalProvidersForStandardRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(courseDirectoryApiResponse);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeNull();
        }
    }
}