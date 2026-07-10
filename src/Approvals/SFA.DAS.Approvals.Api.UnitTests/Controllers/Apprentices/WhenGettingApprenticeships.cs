using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeships;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

public class WhenGettingApprenticeships
{
    [Test, MoqAutoData]
    public async Task Then_Get_Apprenticeships(
           long providerId,
           GetApprenticeshipsQueryResult mediatorResult,
           InnerApi.Requests.GetApprenticeshipsRequest request,
           [Frozen] Mock<IMediator> mockMediator
           )
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();

        var _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), mockMediator.Object, mapper);

        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApprenticeshipsQuery>(),
                It.IsAny<CancellationToken>()))
        .ReturnsAsync(mediatorResult);

        var controllerResult = await _controller.GetApprenticeships(providerId, request) as ObjectResult;

        controllerResult.Should().NotBeNull();

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetApprenticeshipsQueryResult;

        model.Should().NotBeNull();

        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_NotFoundResponse_Is_Returned_If_No_Apprenticeships_Are_Found(
           long providerId,
           GetApprenticeshipsQueryResult mediatorResult,
           InnerApi.Requests.GetApprenticeshipsRequest request,
           [Frozen] Mock<IMediator> mockMediator
           )
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();

        var _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), mockMediator.Object, mapper);

        mockMediator
            .Setup(mediator => mediator.Send(
                It.IsAny<GetApprenticeshipsQuery>(),
                It.IsAny<CancellationToken>()))
        .ReturnsAsync((GetApprenticeshipsQueryResult)null);

        var controllerResult = await _controller.GetApprenticeships(providerId, request) as NotFoundResult;

        controllerResult.Should().NotBeNull();
        controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}