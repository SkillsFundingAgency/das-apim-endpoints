using System.Threading;
using System.Threading.Tasks;
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

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api(
            GetTrainingCourseQuery query,
            GetStandardsListItem apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetTrainingCourseQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListItem>(It.Is<GetStandardRequest>(c=>c.GetUrl.Contains(query.Id.ToString()))))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Course.Should().BeEquivalentTo(apiResponse);
        }
    }
}