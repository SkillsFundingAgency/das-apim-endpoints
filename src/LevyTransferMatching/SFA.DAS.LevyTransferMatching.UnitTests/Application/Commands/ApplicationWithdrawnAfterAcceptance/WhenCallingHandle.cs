using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ApplicationWithdrawnAfterAcceptance;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ApplicationWithdrawnAfterAcceptance
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private readonly Fixture _fixture;

        public WhenCallingHandle()
        {
            _fixture = new Fixture();
        }

        [Test, MoqAutoData]
        public async Task Credit_Pledge_Is_Called(
            ApplicationWithdrawnAfterAcceptanceCommand command,
            [Frozen] Mock<ILevyTransferMatchingService> levyTransferMatchingService,
            ApplicationWithdrawnAfterAcceptanceCommandHandler handler
            )
        {
            await handler.Handle(command, CancellationToken.None);

            levyTransferMatchingService.Verify(x => 
                x.CreditPledge(It.Is<CreditPledgeRequest>(x => x.PostUrl.Contains(command.PledgeId.ToString()) &&
                ((CreditPledgeRequest.CreditPledgeRequestData)x.Data).Amount == command.Amount &&
                ((CreditPledgeRequest.CreditPledgeRequestData)x.Data).ApplicationId == command.ApplicationId)), Times.Exactly(1));
        }
    }
}
