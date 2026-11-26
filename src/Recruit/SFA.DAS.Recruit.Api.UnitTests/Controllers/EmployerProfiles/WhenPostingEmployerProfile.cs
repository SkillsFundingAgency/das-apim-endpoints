using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models.EmployerProfiles;
using SFA.DAS.Recruit.Application.EmployerProfile.Commands.PostEmployerProfile;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerProfiles;

[TestFixture]
internal class WhenPostingEmployerProfile
{
    [Test, MoqAutoData]
    public async Task Then_Post_EmployerProfile_To_Mediator(
        long accountLegalEntityId,
        PostEmployerProfileApiRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerProfileController controller)
    {
        IResult? actual = await controller.PutOne(accountLegalEntityId, request) as Created;

        // assert
        actual.Should().NotBeNull();
        mockMediator.Verify(x => x.Send(It.Is<PostEmployerProfileCommand>(
            c =>
                c.AccountLegalEntityId == accountLegalEntityId
                && c.AccountId == request.AccountId
                && c.AboutOrganisation == request.AboutOrganisation
                && c.TradingName == request.TradingName
        ), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Problem_Request(
        long accountLegalEntityId,
        PostEmployerProfileApiRequest request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerProfileController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<PostEmployerProfileCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var problemResult = await controller.PutOne(accountLegalEntityId, request) as ProblemHttpResult;

        problemResult.Should().NotBeNull();
        problemResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
