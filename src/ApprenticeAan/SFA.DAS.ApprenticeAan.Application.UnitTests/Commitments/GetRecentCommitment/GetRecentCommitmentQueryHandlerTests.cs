using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;
using SFA.DAS.ApprenticeAan.Application.InnerApi.ApprenticeshipsValidate;
using SFA.DAS.ApprenticeAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Commitments.GetRecentCommitment;

public class GetRecentCommitmentQueryHandlerTests
{
    private readonly Fixture _fixture = new();

    [Test, MoqAutoData]
    public async Task Handle_CommitmentNotFound_ReturnsNull(
        [Frozen] Mock<ICommitmentsV2InnerApiClient> clientMock,
        GetRecentCommitmentQueryHandler sut,
        GetRecentCommitmentQuery query,
        CancellationToken cancellationToken)
    {
        clientMock.Setup(c => c.GetApprenticeshipsValidate(query.FirstName, query.LastName, query.DateOfBirth, cancellationToken)).ReturnsAsync(new GetApprenticeshipsValidateResponse());

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_FoundSingleCommitment_ReturnsCommitmentIrrespectiveOfItsState(
        [Frozen] Mock<ICommitmentsV2InnerApiClient> clientMock,
        GetRecentCommitmentQueryHandler sut,
        GetRecentCommitmentQuery query,
        GetRecentCommitmentQueryResult expected,
        CancellationToken cancellationToken)
    {
        var apiResponse = new GetApprenticeshipsValidateResponse() { Apprenticeships = new[] { expected } };
        clientMock.Setup(c => c.GetApprenticeshipsValidate(query.FirstName, query.LastName, query.DateOfBirth, cancellationToken)).ReturnsAsync(apiResponse);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_FoundMultipleActiveCommitments_ReturnsMostRecentActive(
        [Frozen] Mock<ICommitmentsV2InnerApiClient> clientMock,
        GetRecentCommitmentQueryHandler sut,
        GetRecentCommitmentQuery query,
        CancellationToken cancellationToken)
    {
        GetApprenticeshipsValidateResponse apiResponse = new();
        apiResponse.Apprenticeships = new[]
        {
            GetActiveCommitment(_fixture.Create<DateTime>()),
            GetActiveCommitment(_fixture.Create<DateTime>()),
            GetActiveCommitment(_fixture.Create<DateTime>()),
            GetActiveCommitment(_fixture.Create<DateTime>())
        };
        clientMock.Setup(c => c.GetApprenticeshipsValidate(query.FirstName, query.LastName, query.DateOfBirth, cancellationToken)).ReturnsAsync(apiResponse);
        var expected = apiResponse.Apprenticeships.OrderByDescending(a => a.StartDate).First();

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_FoundMultipleCommitmentsWithRecentCancelled_ReturnsMostRecentActive(
        [Frozen] Mock<ICommitmentsV2InnerApiClient> clientMock,
        GetRecentCommitmentQueryHandler sut,
        GetRecentCommitmentQuery query,
        CancellationToken cancellationToken)
    {
        GetApprenticeshipsValidateResponse apiResponse = new();
        DateTime startDate = _fixture.Create<DateTime>();
        var expected = GetActiveCommitment(startDate.AddDays(-2));
        apiResponse.Apprenticeships = new[]
        {
            GetCancelledCommitment(startDate),
            GetStoppedCommitment(startDate.AddDays(-1)),
            expected,
            GetActiveCommitment(startDate.AddMonths(-1))
        };

        clientMock.Setup(c => c.GetApprenticeshipsValidate(query.FirstName, query.LastName, query.DateOfBirth, cancellationToken)).ReturnsAsync(apiResponse);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_FoundMultipleCommitmentsWithAllCancelled_ReturnsMostRecent(
    [Frozen] Mock<ICommitmentsV2InnerApiClient> clientMock,
    GetRecentCommitmentQueryHandler sut,
    GetRecentCommitmentQuery query,
    CancellationToken cancellationToken)
    {
        GetApprenticeshipsValidateResponse apiResponse = new();
        apiResponse.Apprenticeships = new[]
        {
            GetCancelledCommitment(_fixture.Create<DateTime>()),
            GetStoppedCommitment(_fixture.Create<DateTime>())
        };
        clientMock.Setup(c => c.GetApprenticeshipsValidate(query.FirstName, query.LastName, query.DateOfBirth, cancellationToken)).ReturnsAsync(apiResponse);
        var expected = apiResponse.Apprenticeships.OrderByDescending(a => a.StartDate).First();

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }

    private GetRecentCommitmentQueryResult GetStoppedCommitment(DateTime startDate) => _fixture.Build<GetRecentCommitmentQueryResult>().With(c => c.PaymentStatus, 3).With(c => c.StartDate, startDate).Create();

    private GetRecentCommitmentQueryResult GetCancelledCommitment(DateTime startDate) => _fixture.Build<GetRecentCommitmentQueryResult>().With(c => c.StopDate, startDate).With(c => c.StartDate, startDate).Create();

    private GetRecentCommitmentQueryResult GetActiveCommitment(DateTime startDate) => _fixture.Build<GetRecentCommitmentQueryResult>().With(c => c.PaymentStatus, 1).With(c => c.StartDate, startDate).Create();
}
