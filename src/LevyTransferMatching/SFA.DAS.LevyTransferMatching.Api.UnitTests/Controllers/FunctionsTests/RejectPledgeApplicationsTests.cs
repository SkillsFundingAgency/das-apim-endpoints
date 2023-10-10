using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.RejectPledgeApplications;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    public class RejectPledgeApplicationsTests
    {
        [Test]
        [MoqAutoData]
        public async Task Action_Calls_Handler(            
            [Frozen] Mock<IMediator> mediator,
            RejectPledgeApplicationsRequest request,
            [NoAutoProperties] FunctionsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<RejectPledgeApplicationsCommand>(x =>
                   x.PledgeId.Equals(request.PledgeId)
                   ), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(() => new Unit());

            await controller.RejectPledgeApplications(request);

            mediator.Verify(x =>
                x.Send(It.Is<RejectPledgeApplicationsCommand>(c =>
                        c.PledgeId == request.PledgeId),
                    It.IsAny<CancellationToken>()));
        }
    }
}