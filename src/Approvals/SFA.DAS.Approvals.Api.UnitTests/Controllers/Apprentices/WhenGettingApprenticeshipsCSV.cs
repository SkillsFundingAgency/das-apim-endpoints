using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.Api.AppStart;
using SFA.DAS.Approvals.Api.Models.Apprentices;
using SFA.DAS.Approvals.Application.Apprentices.Queries.GetApprenticeshipsCSV;

namespace SFA.DAS.Approvals.Api.UnitTests.Controllers.Apprentices
{
    [TestFixture]
    public class WhenGettingApprenticeshipsCSV
    {
        [Test, MoqAutoData]
        public async Task Then_Get_Apprenticeship_CSV(
           long providerId,
           GetApprenticeshipsCSVQueryResult mediatorResult,
           PostApprenticeshipsCSVRequest request,
           [Frozen] Mock<IMediator> mockMediator
           )
        {
            var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });
            var mapper = mappingConfig.CreateMapper();

            var _controller = new ApprenticesController(Mock.Of<ILogger<ApprenticesController>>(), mockMediator.Object, mapper);

            mockMediator
                .Setup(mediator => mediator.Send(
                    It.IsAny<GetApprenticeshipsCSVQuery>(),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

            var controllerResult = await _controller.GetApprenticeshipsCSV(providerId, request) as ObjectResult;

            controllerResult.Should().NotBeNull();

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as PostApprenticeshipsCSVResponse;

            model.Should().NotBeNull();

            model.Should().BeEquivalentTo(mediatorResult);
        }
    }
}
