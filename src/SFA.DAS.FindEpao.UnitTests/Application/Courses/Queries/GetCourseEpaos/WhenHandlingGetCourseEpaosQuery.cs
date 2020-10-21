using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpaos;
using SFA.DAS.FindEpao.InnerApi.Requests;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.UnitTests.Application.Courses.Queries.GetCourseEpaos
{
    public class WhenHandlingGetCourseEpaosQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Epaos_From_Assessors_Api_And_Course_From_Courses_Api(
            GetCourseEpaosQuery query,
            GetCourseEpaoListResponse epaoApiResponse,
            GetStandardResponse coursesApiResponse,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockCoursesApiClient,
            GetCourseEpaosQueryHandler handler)
        {
            mockAssessorsApiClient
                .Setup(client => client.Get<GetCourseEpaoListResponse>(
                    It.Is<GetCourseEpaosRequest>(request => request.CourseId == query.CourseId)))
                .ReturnsAsync(epaoApiResponse);
            mockCoursesApiClient
                .Setup(client => client.Get<GetStandardResponse>(
                    It.Is<GetStandardRequest>(request => request.StandardId == query.CourseId)))
                .ReturnsAsync(coursesApiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Epaos.Should().BeEquivalentTo(epaoApiResponse.CourseEpaos);
            result.Course.Should().BeEquivalentTo(coursesApiResponse.Standard);
        }
    }
}