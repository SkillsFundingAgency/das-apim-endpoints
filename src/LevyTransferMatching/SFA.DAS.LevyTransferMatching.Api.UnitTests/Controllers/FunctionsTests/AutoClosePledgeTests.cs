using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using MediatR;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.AutoClosePledge;
using SFA.DAS.Testing.AutoFixture;
using AutoFixture.NUnit3;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    public class AutoClosePledgeTests
    {
        [Test]
        [MoqAutoData]
        public async Task AutoClosePledgeReturnsResponse(            
              [Frozen] Mock<IMediator> mediator,
              AutoClosePledgeCommandResult closeResult,
              AutoClosePledgeRequest request,
              [NoAutoProperties] FunctionsController controller
            )
        {
            mediator.Setup(x => x.Send(It.Is<AutoClosePledgeCommand>(x =>
                    x.PledgeId.Equals(request.PledgeId)
                    && x.ApplicationId.Equals(request.ApplicationId)), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(closeResult);

            await controller.AutoClosePledge(request);

            mediator.Verify(x =>
                x.Send(It.Is<AutoClosePledgeCommand>(x =>
                    x.PledgeId.Equals(request.PledgeId)
                    && x.ApplicationId.Equals(request.ApplicationId)),
                    It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}