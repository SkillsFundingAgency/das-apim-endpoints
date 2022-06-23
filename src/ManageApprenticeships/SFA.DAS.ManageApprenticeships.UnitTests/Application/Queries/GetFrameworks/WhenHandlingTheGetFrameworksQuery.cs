using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ManageApprenticeships.Application.Queries.GetFrameworks;
using SFA.DAS.ManageApprenticeships.InnerApi.Requests;
using SFA.DAS.ManageApprenticeships.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ManageApprenticeships.UnitTests.Application.Queries.GetFrameworks
{
    public class WhenHandlingTheGetFrameworksQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Frameworks_From_Courses_Api(
            GetFrameworksQuery query,
            GetFrameworksListResponse apiResponse,
            [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
            GetFrameworksQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetFrameworksListResponse>(It.IsAny<GetFrameworksRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Frameworks.Should().BeEquivalentTo(apiResponse.Frameworks);
        }
    }
}