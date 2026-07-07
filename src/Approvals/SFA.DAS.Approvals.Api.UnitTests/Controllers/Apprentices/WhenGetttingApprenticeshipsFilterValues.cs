using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetApprenticeshipsFilterValues;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices;

public class WhenGetttingApprenticeshipsFilterValues
{
    [Test, MoqAutoData]
    public async Task Then_Get_ApprenticeshipsFilterValues(
           long providerId,
           GetApprenticeshipsFilterValuesQueryResult mediatorResult,
           [Frozen] Mock<IMediator> mockMediator)
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();

        var _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), mockMediator.Object, mapper);

        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipsFilterValuesQuery>(t => t.ProviderId == providerId),
                It.IsAny<CancellationToken>()))
        .ReturnsAsync(mediatorResult);

        var controllerResult = await _controller.GetApprenticeshipsFilterValues(providerId, null) as ObjectResult;

        controllerResult.Should().NotBeNull();

        controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
        var model = controllerResult.Value as GetApprenticeshipsFiltersResponse;

        model.Should().NotBeNull();

        model.Should().BeEquivalentTo(mediatorResult);
    }

    [Test, MoqAutoData]
    public async Task Then_NotFoundResponse_Is_Returned_If_No_ApprenticeshipsFilterValues_Are_Found(
           long providerId,
           [Frozen] Mock<IMediator> mockMediator)
    {
        var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
        var mapper = mappingConfig.CreateMapper();

        var _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), mockMediator.Object, mapper);

        mockMediator
            .Setup(mediator => mediator.Send(
                It.Is<GetApprenticeshipsFilterValuesQuery>(t => t.ProviderId == providerId),
                It.IsAny<CancellationToken>()))
        .ReturnsAsync((GetApprenticeshipsFilterValuesQueryResult)null);

        var controllerResult = await _controller.GetApprenticeshipsFilterValues(providerId, null) as NotFoundResult;

        controllerResult.Should().NotBeNull();
        controllerResult?.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
    }
}