using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.LegalEntity
{
    [TestFixture]
    public class WhenRefreshingLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_RefreshLegalEntitiesCommand_Is_Sent(
            int pageNumber,
            int pageSize,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] LegalEntityController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<RefreshLegalEntitiesCommand>(x =>
                        x.PageNumber == pageNumber && x.PageSize == pageSize),
                    It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.RefreshLegalEntities(pageNumber, pageSize) as OkResult;

            Assert.IsNotNull(controllerResult);
        }
    }
}
