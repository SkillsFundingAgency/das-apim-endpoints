using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Courses.Queries.GetCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Courses.Queries.GetCourseProviders
{
    public class WhenGettingProvidersByTrainingCourse
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Expected_Response(
            GetCourseProvidersResponse apiResponse,
            GetCourseProvidersQuery query,
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> courseManagementApiMock,
            [Greedy] GetTrainingCourseProvidersQueryHandler handler,
            CancellationToken cancellationToken
            )
        {
            courseManagementApiMock
                .Setup(client => client.GetWithResponseCode<GetCourseProvidersResponse>(It.Is<GetProvidersByCourseIdRequest>(
                    c => c.GetUrl.Contains(query.Id.ToString())
                    && c.GetUrl.Contains($"api/courses/{query.Id}/providers")
                    )
                ))
                .ReturnsAsync(new ApiResponse<GetCourseProvidersResponse>(apiResponse,
                    HttpStatusCode.OK, ""));

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().Be(apiResponse);
        }
    }
}