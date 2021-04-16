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
using SFA.DAS.FindApprenticeshipTraining.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

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
            GetShowEmployerDemandResponse showEmployerDemandResponse,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingConfiguration>> config,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            GetTrainingCourseQueryHandler handler)
        {
            //Arrange
            config.Object.Value.EmployerDemandFeatureToggle = true;
            levelsApiResponse.Levels.First().Name = "GCSE";
            levelsApiResponse.Levels.First().Code = 2;
            coursesApiResponse.Level = 2;
            coursesApiResponse.LevelEquivalent = levelsApiResponse.Levels
                .Single(x => x.Code == coursesApiResponse.Level).Name;
            var url = new GetUkprnsForStandardAndLocationRequest(query.Id, query.Lat, query.Lon).GetUrl;

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
            mockEmployerDemandApiClient
                .Setup(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE");
            result.ProvidersCount.Should().Be(courseDirectoryApiResponse.UkprnsByStandard.ToList().Count);
            result.ProvidersCountAtLocation.Should().Be(courseDirectoryApiResponse.UkprnsByStandardAndLocation.ToList().Count);
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
            result.ShowEmployerDemand.Should().BeTrue();
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

        [Test, MoqAutoData]
        public async Task And_ShowDemandResponse_Not_OK_Then_ShowDemand_False(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetUkprnsForStandardAndLocationResponse courseDirectoryApiResponse,
            int shortlistItemCount,
            GetShowEmployerDemandResponse showEmployerDemandResponse,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingConfiguration>> config,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            GetTrainingCourseQueryHandler handler)
        {
            //Arrange
            config.Object.Value.EmployerDemandFeatureToggle = true;
            levelsApiResponse.Levels.First().Name = "GCSE";
            levelsApiResponse.Levels.First().Code = 2;
            coursesApiResponse.Level = 2;
            coursesApiResponse.LevelEquivalent = levelsApiResponse.Levels
                .Single(x => x.Code == coursesApiResponse.Level).Name;
            var url = new GetUkprnsForStandardAndLocationRequest(query.Id, query.Lat, query.Lon).GetUrl;

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
            mockEmployerDemandApiClient
                .Setup(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()))
                .ReturnsAsync(HttpStatusCode.Forbidden);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.ShowEmployerDemand.Should().BeFalse();
        }

        [Test, MoqAutoData]
        public async Task Then_If_Demand_Feature_Is_Not_Enabled_Then_False_Returned_And_Api_Not_Called(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetUkprnsForStandardAndLocationResponse courseDirectoryApiResponse,
            int shortlistItemCount,
            GetShowEmployerDemandResponse showEmployerDemandResponse,
            [Frozen] Mock<IOptions<FindApprenticeshipTrainingConfiguration>> config,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
            [Frozen] Mock<IShortlistService> shortlistService,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            GetTrainingCourseQueryHandler handler)
        {
            //Arrange
            config.Object.Value.EmployerDemandFeatureToggle = false;
            levelsApiResponse.Levels.First().Name = "GCSE";
            levelsApiResponse.Levels.First().Code = 2;
            coursesApiResponse.Level = 2;
            coursesApiResponse.LevelEquivalent = levelsApiResponse.Levels
                .Single(x => x.Code == coursesApiResponse.Level).Name;
            var url = new GetUkprnsForStandardAndLocationRequest(query.Id, query.Lat, query.Lon).GetUrl;

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
            mockEmployerDemandApiClient
                .Setup(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()))
                .ReturnsAsync(HttpStatusCode.OK);

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE");
            result.ProvidersCount.Should().Be(courseDirectoryApiResponse.UkprnsByStandard.ToList().Count);
            result.ProvidersCountAtLocation.Should().Be(courseDirectoryApiResponse.UkprnsByStandardAndLocation.ToList().Count);
            result.ShortlistItemCount.Should().Be(shortlistItemCount);
            result.ShowEmployerDemand.Should().BeFalse();
            mockEmployerDemandApiClient
                .Verify(client => client.GetResponseCode(It.IsAny<GetShowEmployerDemandRequest>()), Times.Never);
        }
    }
}