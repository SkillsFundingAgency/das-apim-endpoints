using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Application.TrainingCourses.Queries.GetTrainingCoursesList;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.TrainingCourses.Queries
{
    public class WhenGettingTrainingCourseList
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api(
            GetTrainingCoursesListQuery query,
            GetStandardsListResponse apiResponse,
            [Frozen] Mock<IApiClient> mockApiClient,
            GetTrainingCoursesListQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<IGetApiRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Courses.Should().BeEquivalentTo(apiResponse.Standards);
        }
    }
}
