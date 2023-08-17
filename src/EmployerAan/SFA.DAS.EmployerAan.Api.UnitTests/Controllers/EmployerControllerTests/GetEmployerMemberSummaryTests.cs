using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMemberSummary;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.EmployerControllerTests;
public class GetEmployerMemberSummaryTests
{
    [Test, MoqAutoData]
    public async Task GetEmployerMemberSummary_InvokesMediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployersController sut,
        int employerAccountId,
        CancellationToken cancellationToken)
    {
        await sut.GetEmployerMemberSummary(employerAccountId, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetEmployerMemberSummaryQuery>(q => q.EmployerAccountId == employerAccountId), cancellationToken));
    }

    [Test, MoqAutoData]
    public async Task GetEmployerMemberSummary_MediatorSuccessful_ReturnResult(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployersController sut,
        int employerAccountId,
        GetEmployerMemberSummaryQueryResult expectedResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberSummaryQuery>(q => q.EmployerAccountId == employerAccountId), cancellationToken)).ReturnsAsync(expectedResult);

        var actualResult = await sut.GetEmployerMemberSummary(employerAccountId, cancellationToken);

        actualResult.As<OkObjectResult>().Value.Should().Be(expectedResult);
    }

    [Test, MoqAutoData]
    public async Task GetEmployerMemberSummary_MediatorUnsuccessful_ReturnsNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployersController sut,
        int employerAccountId,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberSummaryQuery>(q => q.EmployerAccountId == employerAccountId), cancellationToken)).ReturnsAsync((GetEmployerMemberSummaryQueryResult?)null);

        var actualResult = await sut.GetEmployerMemberSummary(employerAccountId, cancellationToken);

        actualResult.As<NotFoundResult>().Should().NotBeNull();
    }
}
