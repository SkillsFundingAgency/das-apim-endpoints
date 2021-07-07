using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_New_PledgeId_Is_Returned(
            CreatePledgeCommand createPledgeCommand,
            long accountId,
            int pledgeId,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            CreatePledgeHandler createPledgeHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetAccount(It.Is<GetAccountRequest>(r => r.AccountId == accountId)))
                .ReturnsAsync(() => new GetAccountResponse());

            mockLevyTransferMatchingService
                .Setup(x => x.CreatePledge(It.Is<Pledge>(y => y.AccountId == accountId)))
                .ReturnsAsync(pledgeId);

            createPledgeCommand.AccountId = accountId;

            var createPledgeResult = await createPledgeHandler.Handle(createPledgeCommand, CancellationToken.None);

            Assert.AreEqual(pledgeId, createPledgeResult.PledgeId);
        }

        [Test, MoqAutoData]
        public async Task Then_Account_Is_Created_If_Not_Already_Exists(
            CreatePledgeCommand createPledgeCommand,
            long accountId,
            int pledgeId,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            CreatePledgeHandler createPledgeHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetAccount(It.Is<GetAccountRequest>(r => r.AccountId == accountId)))
                .ReturnsAsync(() => null);

            mockLevyTransferMatchingService
                .Setup(x => x.CreateAccount(It.Is<CreateAccountRequest>(r => r.AccountId == accountId)))
                .Returns(Task.CompletedTask);


            mockLevyTransferMatchingService
                .Setup(x => x.CreatePledge(It.Is<Pledge>(y => y.AccountId == accountId)))
                .ReturnsAsync(pledgeId);

            createPledgeCommand.AccountId = accountId;

            await createPledgeHandler.Handle(createPledgeCommand, CancellationToken.None);

            mockLevyTransferMatchingService.Verify(x => x.CreateAccount(It.Is<CreateAccountRequest>(r => r.AccountId == createPledgeCommand.AccountId && r.AccountName == createPledgeCommand.DasAccountName)));
        }
    }
}