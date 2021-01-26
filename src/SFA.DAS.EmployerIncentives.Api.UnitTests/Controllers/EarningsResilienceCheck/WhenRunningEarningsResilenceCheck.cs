using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Application.Commands.EarningsResilienceCheck;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.EarningsResilienceCheck
{
    public class WhenRunningEarningsResilenceCheck
    {
        [Test, MoqAutoData]
        public async Task Then_EarningsResilienceCheckCommand_Is_Sent(
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] EarningsResilienceCheckController controller)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<EarningsResilienceCheckCommand>(),
                                           It.IsAny<CancellationToken>())).ReturnsAsync(Unit.Value);

            var controllerResult = await controller.EarningsResilienceCheck() as OkResult;

            Assert.IsNotNull(controllerResult);
        }
    }
}
