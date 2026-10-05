using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetApprenticeship;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

public class WhenGettingAnApprenticeship
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Apprenticeship_From_Mediator(
           long apprenticeshipId,
           GetApprenticeshipQueryResult mediatorResult,
           [Frozen] Mock<IMediator> mockMediator)
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();

        var _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), mockMediator.Object, mapper);

        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipQuery>(x => x.ApprenticeshipId == apprenticeshipId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        var controllerResult = await _controller.GetApprenticeship(apprenticeshipId) as ObjectResult;

        controllerResult.Should().NotBeNull();
        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetApprenticeshipResponse;
        model.Should().NotBeNull();
        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task And_Then_No_Apprenticeship_Is_Returned_From_Mediator(
        long apprenticeshipId,
        [Frozen] Mock<IMediator> mockMediator,
        [Greedy] ApprenticesController controller)
    {
        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApprenticeshipQuery>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(() => null);

        var controllerResult = await controller.GetApprenticeship(apprenticeshipId) as NotFoundResult;

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}