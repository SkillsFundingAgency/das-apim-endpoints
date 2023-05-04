using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.EmployerControllerTests;
public class GetEmployerMemberTests
{
    [Test]
    [MoqAutoData]
    public async Task GetEmployer_InvokesMediator(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] EmployersController sut,
        Guid userRef,
        CancellationToken cancellationToken)
    {
        await sut.GetEmployerMember(userRef, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef), cancellationToken));
    }

    [Test]
    [MoqAutoData]
    public async Task GetEmployer_MediatorSuccessful_ReturnResult(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] EmployersController sut,
    Guid userRef,
    CancellationToken cancellationToken,
    GetEmployerMemberQueryResult expectedResult)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef), cancellationToken)).ReturnsAsync(expectedResult);

        var actualResult = await sut.GetEmployerMember(userRef, cancellationToken);

        actualResult.As<OkObjectResult>().Value.Should().Be(expectedResult);
    }

    [Test]
    [MoqAutoData]
    public async Task GetEmployer_MediatorUnsuccessful_ReturnsNotFound(
    [Frozen] Mock<IMediator> mediatorMock,
    [Greedy] EmployersController sut,
    Guid userRef,
    CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetEmployerMemberQuery>(q => q.UserRef == userRef), cancellationToken)).ReturnsAsync((GetEmployerMemberQueryResult?)null);

        var actualResult = await sut.GetEmployerMember(userRef, cancellationToken);

        actualResult.As<NotFoundResult>().Should().NotBeNull();
    }
}
