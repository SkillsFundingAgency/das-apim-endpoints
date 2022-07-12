using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourse;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.UnitTests.Application.Courses.Queries.GetCourse
{
    public class WhenHandlingGetCourseQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standard_From_Courses_Api(
            GetCourseQuery query,
            GetStandardsListItem apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetCourseQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.IsAny<GetStandardRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Course.Should().BeEquivalentTo(apiResponse);
        }
    }
}