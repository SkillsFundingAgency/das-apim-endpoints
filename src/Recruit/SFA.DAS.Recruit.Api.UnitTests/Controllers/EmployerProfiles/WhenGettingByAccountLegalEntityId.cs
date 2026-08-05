using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.EmployerProfile.Queries.GetEmployerProfileByLegalEntityId;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerProfiles;

[TestFixture]
internal class WhenGettingByAccountLegalEntityId
{
    [Test, MoqAutoData]
    public async Task Then_Get_EmployerProfile_From_Mediator(
        long accountLegalEntityId,
        GetEmployerProfileByLegalEntityIdQueryResult mediatorResult,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerProfileController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetEmployerProfileByLegalEntityIdQuery>(c => c.AccountLegalEntityId.Equals(accountLegalEntityId)),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        IResult? controllerResult = await controller.GetOne(accountLegalEntityId);

        Assert.That(controllerResult, Is.Not.Null);
        var payload = (controllerResult as Ok<GetEmployerProfileByLegalEntityIdQueryResult>)?.Value;

        // assert
        payload.Should().NotBeNull();
        payload.Should().BeEquivalentTo(mediatorResult, options => options.ExcludingMissingMembers());
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Problem_Request(
        long accountLegalEntityId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerProfileController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetEmployerProfileByLegalEntityIdQuery>(c => c.AccountLegalEntityId.Equals(accountLegalEntityId)),
                It.IsAny<CancellationToken>()))
           .Throws<InvalidOperationException>();

        IResult? controllerResult = await controller.GetOne(accountLegalEntityId);

        controllerResult.Should().NotBeNull();

        var problemResult = controllerResult as ProblemHttpResult;

        problemResult.Should().NotBeNull();
        problemResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
