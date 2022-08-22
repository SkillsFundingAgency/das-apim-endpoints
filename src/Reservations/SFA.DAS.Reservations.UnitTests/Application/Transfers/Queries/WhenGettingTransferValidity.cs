using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.Transfers.Queries.GetTransferValidity;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.Transfers.Queries
{
    [TestFixture]
    public class WhenGettingTransferValidity
    {
        [Test, MoqAutoData]
        public async Task When_PledgeApplicationId_Has_Value_Return_True_If_PledgeApplication_Is_Accepted(
            GetTransferValidityQuery query,
            GetPledgeApplicationResponse pledgeApplication,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockAccountsApiClient,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockApiClient,
            GetTransferValidityQueryHandler handler)
        {
            pledgeApplication.ReceiverEmployerAccountId = query.ReceiverId;
            pledgeApplication.SenderEmployerAccountId = query.SenderId;
            pledgeApplication.Status = "Accepted";

            mockApiClient
                .Setup(client => client.Get<GetPledgeApplicationResponse>(It.IsAny<GetPledgeApplicationRequest>()))
                .ReturnsAsync(pledgeApplication);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsTrue(result.IsValid);
        }

        [Test, MoqAutoData]
        public async Task When_PledgeApplicationId_Has_Value_Return_False_If_PledgeApplication_Is_Not_Accepted(
            GetTransferValidityQuery query,
            GetPledgeApplicationResponse pledgeApplication,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockEmployerFinanceApiClient,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockApiClient,
            GetTransferValidityQueryHandler handler)
        {
            pledgeApplication.ReceiverEmployerAccountId = query.ReceiverId;
            pledgeApplication.SenderEmployerAccountId = query.SenderId;
            pledgeApplication.Status = "Declined";

            mockApiClient
                .Setup(client => client.Get<GetPledgeApplicationResponse>(It.IsAny<GetPledgeApplicationRequest>()))
                .ReturnsAsync(pledgeApplication);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsFalse(result.IsValid);
        }

        [Test, MoqAutoData]
        public async Task When_PledgeApplicationId_Has_No_Value_Return_True_If_TransferConnection_Exists(
            GetTransferValidityQuery query,
            List<TransferConnection> transferConnections,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockEmployerFinanceApiClient,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockApiClient,
            GetTransferValidityQueryHandler handler)
        {
            query.PledgeApplicationId = null;

            transferConnections = new List<TransferConnection>
            {
                new TransferConnection{ FundingEmployerAccountId = query.SenderId }
            };

            mockEmployerFinanceApiClient
                .Setup(client => client.GetAll<TransferConnection>(It.IsAny<GetTransferConnectionsRequest>()))
                .ReturnsAsync(transferConnections);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsTrue(result.IsValid);
        }


        [Test, MoqAutoData]
        public async Task When_PledgeApplicationId_Has_No_Value_Return_False_If_TransferConnection_Does_Not_Exist(
            GetTransferValidityQuery query,
            List<TransferConnection> transferConnections,
            [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> mockEmployerFinanceApiClient,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockApiClient,
            GetTransferValidityQueryHandler handler)
        {
            query.PledgeApplicationId = null;
            transferConnections = new List<TransferConnection>();

            mockEmployerFinanceApiClient
                .Setup(client => client.GetAll<TransferConnection>(It.IsAny<GetTransferConnectionsRequest>()))
                .ReturnsAsync(transferConnections);

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.IsFalse(result.IsValid);
        }
    }
}
