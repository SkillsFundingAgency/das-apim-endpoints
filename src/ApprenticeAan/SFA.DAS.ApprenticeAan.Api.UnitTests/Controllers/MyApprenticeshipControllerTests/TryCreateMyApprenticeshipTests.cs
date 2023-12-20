using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Queries.GetMyApprenticeship;
using SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.MyApprenticeshipControllerTests;

public class TryCreateMyApprenticeshipTests
{
    [Test]
    [MoqAutoData]
    public async Task QueriesMyApprenticeship_MyApprenticeshipFound_ReturnsSuccessResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        TryCreateMyApprenticeshipRequestModel model,
        GetMyApprenticeshipQueryResult myApprenticeship,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken)).ReturnsAsync(myApprenticeship);

        var response = await sut.TryCreateMyApprenticeship(model, cancellationToken);

        response.As<OkObjectResult>().Value.As<GetMyApprenticeshipQueryResult>().Should().NotBeNull();
        mediatorMock.Verify(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken), Times.Once);
        mediatorMock.Verify(m => m.Send(It.IsAny<GetRecentCommitmentQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        mediatorMock.Verify(m => m.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [MoqAutoData]
    public async Task MyApprenticeshipNotFound_QueriesCommitments(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        TryCreateMyApprenticeshipRequestModel model,
        GetRecentCommitmentQueryResult commitment,
        CancellationToken cancellationToken)
    {
        commitment.Uln = "1";
        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken)).ReturnsAsync(() => null);
        mediatorMock.Setup(m => m.Send(It.Is<GetRecentCommitmentQuery>(q => q.FirstName == model.FirstName && q.LastName == model.LastName && q.DateOfBirth == model.DateOfBirth), cancellationToken)).ReturnsAsync(commitment);

        var response = await sut.TryCreateMyApprenticeship(model, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.IsAny<GetRecentCommitmentQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        mediatorMock.Verify(m => m.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Test]
    [MoqAutoData]
    public async Task CommitmentFound_CreatesMyApprenticeshipFromCommitments_ReturnsSuccessResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        TryCreateMyApprenticeshipRequestModel model,
        GetRecentCommitmentQueryResult commitment,
        GetMyApprenticeshipQueryResult myApprenticeship,
        CancellationToken cancellationToken)
    {
        commitment.Uln = "1";
        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken)).ReturnsAsync(() =>
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken)).ReturnsAsync(myApprenticeship);
            return null;
        });
        mediatorMock.Setup(m => m.Send(It.Is<GetRecentCommitmentQuery>(q => q.FirstName == model.FirstName && q.LastName == model.LastName && q.DateOfBirth == model.DateOfBirth), cancellationToken)).ReturnsAsync(commitment);

        var response = await sut.TryCreateMyApprenticeship(model, cancellationToken);

        response.As<OkObjectResult>().Value.As<GetMyApprenticeshipQueryResult>().Should().Be(myApprenticeship);
        mediatorMock.Verify(m => m.Send(It.Is<CreateMyApprenticeshipCommand>(c => c.ApprenticeId == model.ApprenticeId && c.ApprenticeshipId == commitment.ApprenticeshipId), cancellationToken));
        mediatorMock.Verify(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken), Times.Exactly(2));
    }

    [Test]
    [MoqAutoData]
    public async Task CommitmentNotFound_QueriesStagedApprentice(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        TryCreateMyApprenticeshipRequestModel model,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken)).ReturnsAsync(() => null);
        mediatorMock.Setup(m => m.Send(It.Is<GetRecentCommitmentQuery>(q => q.FirstName == model.FirstName && q.LastName == model.LastName && q.DateOfBirth == model.DateOfBirth), cancellationToken)).ReturnsAsync(() => null);

        var response = await sut.TryCreateMyApprenticeship(model, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.IsAny<GetStagedApprenticeQuery>(), It.IsAny<CancellationToken>()));
    }

    [Test]
    [MoqAutoData]
    public async Task StagedApprenticeFound_CreatesMyApprenticeshipFromStagedApprentice_ReturnsSuccessResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        TryCreateMyApprenticeshipRequestModel model,
        GetStagedApprenticeResponse stagedApprentice,
        GetMyApprenticeshipQueryResult myApprenticeship,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken)).ReturnsAsync(() =>
        {
            mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken)).ReturnsAsync(myApprenticeship);
            return null;
        });
        mediatorMock.Setup(m => m.Send(It.Is<GetRecentCommitmentQuery>(q => q.FirstName == model.FirstName && q.LastName == model.LastName && q.DateOfBirth == model.DateOfBirth), cancellationToken)).ReturnsAsync(() => null);
        mediatorMock.Setup(m => m.Send(It.Is<GetStagedApprenticeQuery>(q => q.LastName == model.LastName && q.Email == model.Email && q.DateOfBirth == model.DateOfBirth), cancellationToken)).ReturnsAsync(stagedApprentice);

        var response = await sut.TryCreateMyApprenticeship(model, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<CreateMyApprenticeshipCommand>(c => c.ApprenticeId == model.ApprenticeId && c.ApprenticeshipId == stagedApprentice.ApprenticeshipId), cancellationToken));
        response.As<OkObjectResult>().Value.As<GetMyApprenticeshipQueryResult>().Should().Be(myApprenticeship);
        mediatorMock.Verify(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken), Times.Exactly(2));
    }

    [Test]
    [MoqAutoData]
    public async Task MyApprenticeshipNotFound_CommitmentsNotFound_StagedApprenticeFound_ReturnsNotFoundResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] MyApprenticeshipController sut,
        TryCreateMyApprenticeshipRequestModel model,
        GetStagedApprenticeResponse stagedApprentice,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetMyApprenticeshipQuery>(q => q.ApprenticeId == model.ApprenticeId), cancellationToken)).ReturnsAsync(() => null);
        mediatorMock.Setup(m => m.Send(It.Is<GetRecentCommitmentQuery>(q => q.FirstName == model.FirstName && q.LastName == model.LastName && q.DateOfBirth == model.DateOfBirth), cancellationToken)).ReturnsAsync(() => null);
        mediatorMock.Setup(m => m.Send(It.Is<GetStagedApprenticeQuery>(q => q.LastName == model.LastName && q.Email == model.Email && q.DateOfBirth == model.DateOfBirth), cancellationToken)).ReturnsAsync(() => null);

        var response = await sut.TryCreateMyApprenticeship(model, cancellationToken);

        response.As<NotFoundResult>().Should().NotBeNull();
    }
}
