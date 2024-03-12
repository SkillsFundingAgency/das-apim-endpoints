using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.SectorSubjectArea.Queries.GetAllSectorSubjectAreaTier1;

public class GetAllSectorSubjectAreaTier1QueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handler_InvokesApiClient(
        ApiResponse<GetAllSectorSubjectAreaTier1Response> expectedResult,
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> mockApiClient,
        [Greedy] GetAllSectorSubjectAreaTier1QueryHandler sut)
    {
        mockApiClient.Setup(c => c.GetWithResponseCode<GetAllSectorSubjectAreaTier1Response>(It.IsAny<GetAllSectorSubjectAreaTier1Request>())).ReturnsAsync(expectedResult);

        var actualResult = await sut.Handle(It.IsAny<GetAllSectorSubjectAreaTier1Query>(), It.IsAny<CancellationToken>());

        mockApiClient.Verify(c => c.GetWithResponseCode<GetAllSectorSubjectAreaTier1Response>(It.IsAny<GetAllSectorSubjectAreaTier1Request>()), Times.Once);
        actualResult.Should().Be(expectedResult);
    }
}
