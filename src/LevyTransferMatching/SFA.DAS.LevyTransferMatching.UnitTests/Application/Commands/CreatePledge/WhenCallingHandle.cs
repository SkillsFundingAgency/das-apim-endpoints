using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreatePledge;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.Models.Constants;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreatePledge
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_Pledge_Created_And_Pledge_Id_Returned(
            CreatePledgeCommand createPledgeCommand,
            long accountId,
            CreatePledgeResponse response,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            CreatePledgeHandler createPledgeHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetAccount(It.Is<GetAccountRequest>(r => r.AccountId == accountId)))
                .ReturnsAsync(() => new GetAccountResponse());

            mockLevyTransferMatchingService
                .Setup(x => x.CreatePledge(It.IsAny<CreatePledgeRequest>()))
                .ReturnsAsync(response);

            createPledgeCommand.AccountId = accountId;

            var result = await createPledgeHandler.Handle(createPledgeCommand, CancellationToken.None);

            Assert.AreEqual(result.PledgeId, response.Id);
        }

        [Test, MoqAutoData]
        public async Task Then_Account_Is_Created_If_Not_Already_Exists(
            CreatePledgeCommand createPledgeCommand,
            long accountId,
            CreatePledgeResponse response,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            CreatePledgeHandler createPledgeHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetAccount(It.Is<GetAccountRequest>(r => r.AccountId == accountId)))
                .ReturnsAsync(() => null);

            mockLevyTransferMatchingService
                .Setup(x => x.CreateAccount(It.IsAny<CreateAccountRequest>()))
                .Returns(Task.CompletedTask);

            mockLevyTransferMatchingService
                .Setup(x => x.CreatePledge(It.IsAny<CreatePledgeRequest>()))
                .ReturnsAsync(response);

            createPledgeCommand.AccountId = accountId;

            await createPledgeHandler.Handle(createPledgeCommand, CancellationToken.None);

            mockLevyTransferMatchingService.Verify(x => x.CreateAccount(It.Is<CreateAccountRequest>(r =>
                ((CreateAccountRequest.CreateAccountRequestData)r.Data).AccountId == createPledgeCommand.AccountId
                && ((CreateAccountRequest.CreateAccountRequestData)r.Data).AccountName == createPledgeCommand.DasAccountName)));
        }

        [Test, MoqAutoData]
        public async Task Then_If_AutomaticApprovalOption_Is_Not_Specified_Then_Default_Option_Is_Submitted(
            CreatePledgeCommand createPledgeCommand,
            long accountId,
            CreatePledgeResponse response,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            CreatePledgeHandler createPledgeHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetAccount(It.Is<GetAccountRequest>(r => r.AccountId == accountId)))
                .ReturnsAsync(() => new GetAccountResponse());

            mockLevyTransferMatchingService
                .Setup(x => x.CreatePledge(It.IsAny<CreatePledgeRequest>()))
                .ReturnsAsync(response);

            createPledgeCommand.AccountId = accountId;
            createPledgeCommand.AutomaticApprovalOption = string.Empty;

            await createPledgeHandler.Handle(createPledgeCommand, CancellationToken.None);

            mockLevyTransferMatchingService.Verify(x => x.CreatePledge(It.Is<CreatePledgeRequest>(r =>
                ((CreatePledgeRequest.CreatePledgeRequestData)r.Data).AutomaticApprovalOption ==
                AutomaticApprovalOption.Default)));
        }
    }
}