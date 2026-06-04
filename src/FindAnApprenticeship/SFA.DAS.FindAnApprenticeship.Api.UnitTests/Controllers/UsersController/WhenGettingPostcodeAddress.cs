using System.Net;
using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidatePostcodeAddress;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController;
public class WhenGettingPostcodeAddress
{
    [Test, MoqAutoData]
    public async Task And_An_Exception_Is_Thrown_Then_Returns_InternalServerError(
        string postcode,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidatePostcodeAddressQuery>(x => x.Postcode == postcode), CancellationToken.None))
            .ThrowsAsync(new Exception());

        var actual = await controller.PostcodeAddress(postcode) as StatusCodeResult;

        actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Ok_Result(
        string postcode,
        GetCandidatePostcodeAddressQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.UsersController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetCandidatePostcodeAddressQuery>(x => x.Postcode == postcode), CancellationToken.None))
            .ReturnsAsync(queryResult);

        var actual = await controller.PostcodeAddress(postcode);

        actual.Should().BeOfType<OkObjectResult>();
    }
}
