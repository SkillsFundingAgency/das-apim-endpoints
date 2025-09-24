using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerDataJobs.UnitTests.Application.Commands;

public class AssignApprenticeshipIdCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_AssigningApprenticeshipId_Returns_200_Range_HttpStatusCode(
        AssignApprenticeshipIdCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] AssignApprenticeshipIdCommandHandler handler)
    {

        var expectedUrl =
            $"providers/{command.ProviderId}/learners/{command.LearnerDataId}/apprenticeshipId";
        client.Setup(x =>
                x.PatchWithResponseCode(
                    It.Is<PatchLearnerDataApprenticeshipIdRequest>(p => p.Data == command.PatchRequest && p.PatchUrl == expectedUrl)))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.OK, ""));

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Then_AssigningApprenticeshipId_Returns_Non200_Range_HttpStatusCode(
        AssignApprenticeshipIdCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] AssignApprenticeshipIdCommandHandler handler)
    {
        client.Setup(x =>
                x.PatchWithResponseCode(
                    It.IsAny<PatchLearnerDataApprenticeshipIdRequest>()))
            .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.BadRequest, "Error"));

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_AssigningApprenticeshipId_Throws_Exception(
        AssignApprenticeshipIdCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] AssignApprenticeshipIdCommandHandler handler)
    {
        client.Setup(x =>
                x.PatchWithResponseCode(
                    It.IsAny<PatchLearnerDataApprenticeshipIdRequest>()))
            .ThrowsAsync(new ApplicationException("test"));

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationException>().WithMessage("test");
    }
}