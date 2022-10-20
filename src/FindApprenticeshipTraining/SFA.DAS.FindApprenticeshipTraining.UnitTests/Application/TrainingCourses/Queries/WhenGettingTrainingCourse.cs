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
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.FindApprenticeshipTraining.Services;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api_With_Totals_And_Shortlist_Count(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetUkprnsForStandardAndLocationResponse courseDirectoryApiResponse,
            int shortlistItemCount,
            LocationItem locationItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
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
            var url = new GetUkprnsForStandardAndLocationRequest(query.Id, locationItem.GeoPoint.First(), locationItem.GeoPoint.Last()).GetUrl;

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync(coursesApiResponse);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            mockCourseDeliveryApiClient
                .Setup(client =>
                    client.Get<GetUkprnsForStandardAndLocationResponse>(
                        It.Is<GetUkprnsForStandardAndLocationRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(courseDirectoryApiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE");
            result.ProvidersCount.Should().Be(courseDirectoryApiResponse.UkprnsByStandard.ToList().Count);
            result.ProvidersCountAtLocation.Should().Be(courseDirectoryApiResponse.UkprnsByStandardAndLocation.ToList().Count);
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
        }

        [Test, MoqAutoData]
        public async Task Then_If_There_Is_No_Location_Then_Location_Not_Passed(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetUkprnsForStandardAndLocationResponse courseDirectoryApiResponse,
            int shortlistItemCount,
            LocationItem locationItem,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
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
            var url = new GetUkprnsForStandardAndLocationRequest(query.Id, 0, 0).GetUrl;

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync(coursesApiResponse);
            shortlistService.Setup(x => x.GetShortlistItemCount(query.ShortlistUserId))
                .ReturnsAsync(shortlistItemCount);
            mockCourseDeliveryApiClient
                .Setup(client =>
                    client.Get<GetUkprnsForStandardAndLocationResponse>(
                        It.Is<GetUkprnsForStandardAndLocationRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(courseDirectoryApiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE");
            result.ProvidersCount.Should().Be(courseDirectoryApiResponse.UkprnsByStandard.ToList().Count);
            result.ProvidersCountAtLocation.Should().Be(courseDirectoryApiResponse.UkprnsByStandardAndLocation.ToList().Count);
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