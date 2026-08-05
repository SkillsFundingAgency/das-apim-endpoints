using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Application.EmployerProfile.Commands.PatchEmployerProfile;
using SFA.DAS.Recruit.InnerApi.Models;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.EmployerProfiles;

[TestFixture]
internal class WhenPatchingEmployerProfile
{
    [Test, MoqAutoData]
    public async Task Then_Post_EmployerProfile_To_Mediator(
        EmployerProfile request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerProfileController controller)
    {
        IResult? actual = await controller.PatchOne(request) as NoContent;

        // assert
        actual.Should().NotBeNull();
        mockMediator.Verify(x => x.Send(It.Is<PatchEmployerProfileCommand>(
            c =>
                c.AccountLegalEntityId == request.AccountLegalEntityId
                && c.EmployerProfile == request
        ), CancellationToken.None), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task And_Exception_Then_Returns_Problem_Request(
        EmployerProfile request,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] EmployerProfileController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<PatchEmployerProfileCommand>(),
                It.IsAny<CancellationToken>()))
            .Throws<InvalidOperationException>();

        var problemResult = await controller.PatchOne(request) as ProblemHttpResult;

        problemResult.Should().NotBeNull();
        problemResult!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }
}
