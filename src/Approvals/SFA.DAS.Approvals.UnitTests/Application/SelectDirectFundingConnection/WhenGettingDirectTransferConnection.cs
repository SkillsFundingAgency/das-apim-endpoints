﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Azure.Amqp.Framing;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.SelectDirectTransferConnection.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.SelectDirectFundingConnection;

public class WhenGettingDirectTransferConnection
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_To_GetSelectDirectTransferConnection_Returns_ExpectedValues(
        GetSelectDirectTransferConnectionQuery query,
        GetAccountResponse accountResponse,
        IEnumerable<GetTransferConnectionsResponse.TransferConnection> directTransfersResponse,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> reservationsApiClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        GetSelectDirectTransferConnectionQueryHandler handler
    )
    {
        reservationsApiClient.Setup(x =>
                x.Get<GetAccountResponse>(
                    It.Is<GetAccountRequest>(x =>
                        x.HashedAccountId == query.AccountId.ToString())))
            .ReturnsAsync(accountResponse);

        financeApiClient.Setup(x =>
                x.Get<IEnumerable<GetTransferConnectionsResponse.TransferConnection>>(
                    It.Is<GetTransferConnectionsRequest>(x => x.AccountId == query.AccountId)))
            .ReturnsAsync(directTransfersResponse);


        var actual = await handler.Handle(query, CancellationToken.None);

        actual.IsLevyAccount.Should().BeFalse();
        actual.TransferConnections.Should().BeEquivalentTo(directTransfersResponse);
    }
}