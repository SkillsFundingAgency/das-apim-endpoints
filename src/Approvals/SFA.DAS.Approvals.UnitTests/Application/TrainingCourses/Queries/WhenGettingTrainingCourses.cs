using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.TrainingCourses.Queries;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SFA.DAS.Approvals.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourses
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Only_NewCourses_From_Courses_Api(
            GetCoursesQuery query,
            GetCoursesListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetCoursesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetCoursesExportRequest>()))
                .ReturnsAsync(apiResponse);
            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetOldCoursesRequest>()))
                .ReturnsAsync(new GetCoursesListResponse { Courses = new List<GetCoursesListItem>()});

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Courses);
        }

        [Test, MoqAutoData]
        public async Task Then_OldCourses_Compress_ToASingle_LarsVersion(
            GetCoursesQuery query,
            GetCoursesListResponse apiResponseForNewCourses,
            GetCoursesListResponse apiResponseForOldCourses,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetCoursesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetCoursesExportRequest>()))
                .ReturnsAsync(new GetCoursesListResponse { Courses = new List<GetCoursesListItem>() });

            var oldCourses = apiResponseForOldCourses.Courses.ToList();

            for (var i = 1; i < 3; i++)
            {
                oldCourses[i].LarsCode = oldCourses[0].LarsCode;
            }

            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetOldCoursesRequest>()))
                .ReturnsAsync(apiResponseForOldCourses);

            var result = await handler.Handle(query, CancellationToken.None);

            var courses = result.Courses.ToList();
            courses.Count.Should().Be(1);
            courses[0].LarsCode.Should().Be(oldCourses[0].LarsCode);
        }

        [Test, MoqAutoData]
        public async Task Then_OldCourses_Compress_ToASingle_LarsVersion_AndTakes_EffectiveTo_Null_Date_As_Latest(
            GetCoursesQuery query,
            GetCoursesListResponse apiResponseForNewCourses,
            GetCoursesListResponse apiResponseForOldCourses,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetCoursesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetCoursesExportRequest>()))
                .ReturnsAsync(new GetCoursesListResponse { Courses = new List<GetCoursesListItem>() });

            var oldCourses = apiResponseForOldCourses.Courses.ToList();
            oldCourses[1].CourseDates.EffectiveTo = null;
            for (var i = 1; i < 3; i++)
            {
                oldCourses[i].LarsCode = oldCourses[0].LarsCode;
            }

            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetOldCoursesRequest>()))
                .ReturnsAsync(apiResponseForOldCourses);

            var result = await handler.Handle(query, CancellationToken.None);

            var courses = result.Courses.ToList();
            courses.Count.Should().Be(1);
            courses[0].LarsCode.Should().Be(oldCourses[0].LarsCode);
            courses[0].CourseDates.EffectiveTo.Should().BeNull();
        }

        [Test, MoqAutoData]
        public async Task Then_OldCourses_Compress_ToASingle_LarsVersion_AndTakes_Latest_EffectiveTo_Date(
            GetCoursesQuery query,
            GetCoursesListResponse apiResponseForNewCourses,
            GetCoursesListResponse apiResponseForOldCourses,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetCoursesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetCoursesExportRequest>()))
                .ReturnsAsync(new GetCoursesListResponse { Courses = new List<GetCoursesListItem>() });

            var oldCourses = apiResponseForOldCourses.Courses.ToList();
            oldCourses[1].CourseDates.EffectiveTo = DateTime.MaxValue;
            for (var i = 1; i < 3; i++)
            {
                oldCourses[i].LarsCode = oldCourses[0].LarsCode;
            }

            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetOldCoursesRequest>()))
                .ReturnsAsync(apiResponseForOldCourses);

            var result = await handler.Handle(query, CancellationToken.None);

            var courses = result.Courses.ToList();
            courses.Count.Should().Be(1);
            courses[0].LarsCode.Should().Be(oldCourses[0].LarsCode);
            courses[0].CourseDates.EffectiveTo.Should().Be(DateTime.MaxValue);
        }

        [Test, MoqAutoData]
        public async Task Then_NewCourses_Get_EarliestEffectiveFrom_From_OldCourses_And_Not_Return_OldCourses(
            GetCoursesQuery query,
            GetCoursesListResponse apiResponseForNewCourses,
            GetCoursesListResponse apiResponseForOldCourses,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetCoursesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetCoursesExportRequest>()))
                .ReturnsAsync(apiResponseForNewCourses);

            var oldCourses = apiResponseForOldCourses.Courses.ToList();
            var newCourses = apiResponseForNewCourses.Courses.ToList();

            for (var i = 0; i < 3; i++)
            {
                oldCourses[i].LarsCode = newCourses[i].LarsCode;
            }

            mockApiClient
                .Setup(client => client.Get<GetCoursesListResponse>(It.IsAny<GetOldCoursesRequest>()))
                .ReturnsAsync(apiResponseForOldCourses);

            var result = await handler.Handle(query, CancellationToken.None);

            var courses = result.Courses.ToList();
            courses[0].CourseDates.EffectiveFrom.Should().Be(oldCourses[0].CourseDates.EffectiveFrom);
            courses[1].CourseDates.EffectiveFrom.Should().Be(oldCourses[1].CourseDates.EffectiveFrom);
            courses[2].CourseDates.EffectiveFrom.Should().Be(oldCourses[2].CourseDates.EffectiveFrom);
            courses.Count.Should().Be(3);
        }
    }
}