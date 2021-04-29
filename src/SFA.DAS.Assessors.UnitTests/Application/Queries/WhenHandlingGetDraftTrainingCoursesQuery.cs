using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Assessors.Application.Queries.GetTrainingCourses;
using SFA.DAS.Assessors.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingGetDraftTrainingCoursesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Standards_From_Courses_Api(
            GetDraftTrainingCoursesQuery query,
            GetStandardsListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetDraftTrainingCoursesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetStandardsListResponse>(It.IsAny<GetNotYetApprovedStandardsRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.TrainingCourses.Should().BeEquivalentTo(apiResponse.Standards);
        }
    }
}