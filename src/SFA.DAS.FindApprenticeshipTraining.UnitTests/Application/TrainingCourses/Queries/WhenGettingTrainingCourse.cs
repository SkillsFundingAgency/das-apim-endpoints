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

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api_With_Totals(
            GetTrainingCourseQuery query,
            GetStandardsListItem coursesApiResponse,
            GetLevelsListResponse levelsApiResponse,
            GetUkprnsForStandardAndLocationResponse courseDirectoryApiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            [Frozen] Mock<ICourseDeliveryApiClient<CourseDeliveryApiConfiguration>> mockCourseDeliveryApiClient,
            GetTrainingCourseQueryHandler handler)
        {

            //Arrange

            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c => c.GetUrl.Contains($"api/courses/standards/{query.Id}"))))
                .ReturnsAsync(coursesApiResponse);


            var url = new GetUkprnsForStandardAndLocationRequest(query.Id, query.Lat, query.Lon).GetUrl;
            mockCourseDeliveryApiClient
                .Setup(client =>
                    client.Get<GetUkprnsForStandardAndLocationResponse>(
                        It.Is<GetUkprnsForStandardAndLocationRequest>((c =>
                            c.GetUrl.Equals(url)))))
                .ReturnsAsync(courseDirectoryApiResponse);

            mockCoursesApiClient
                .Setup(client => client.Get<GetLevelsListResponse>(It.IsAny<GetLevelsListRequest>()))
                .ReturnsAsync(levelsApiResponse);

            levelsApiResponse.Levels.First().Name = "GCSE";
            levelsApiResponse.Levels.First().Code = 2;
            coursesApiResponse.Level = 2;
            coursesApiResponse.LevelEquivalent = levelsApiResponse.Levels
                .Single(x => x.Code == coursesApiResponse.Level).Name;

            //Act
            var result = await handler.Handle(query, CancellationToken.None);

            //Assert
            result.Course.Should().BeEquivalentTo(coursesApiResponse);
            result.Course.LevelEquivalent.Should().Be("GCSE");
            result.ProvidersCount.Should().Be(courseDirectoryApiResponse.UkprnsByStandard.ToList().Count);
            result.ProvidersCountAtLocation.Should().Be(courseDirectoryApiResponse.UkprnsByStandardAndLocation.ToList().Count);
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