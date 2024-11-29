using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.SelectFunding.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.SelectFundingOptions;

public class WhenGettingFundingOption
{
    [Test, MoqAutoData]
    public async Task ThenDirectTransfersAreAvailable_WhenTransferConnectionsExist(
        GetSelectFundingOptionsQuery query,
        GetAccountReservationsStatusResponse reservationsResponse,
        GetTransferConnectionsResponse directTransfersResponse,
        GetAccountResponse accountResponse,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsApiClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        reservationsApiClient.Setup(x =>
                x.Get<GetAccountReservationsStatusResponse>(
                    It.Is<GetAccountReservationsStatusRequest>(x =>
                        x.AccountId == query.AccountId && x.TransferSenderId == null)))
            .ReturnsAsync(reservationsResponse);

        financeApiClient.Setup(x =>
                x.Get<GetTransferConnectionsResponse>(
                    It.Is<GetTransferConnectionsRequest>(x => x.AccountId == query.AccountId)))
            .ReturnsAsync(directTransfersResponse);

        accountsApiClient.Setup(x =>
                x.Get<GetAccountResponse>(
                    It.Is<GetAccountRequest>(x => x.HashedAccountId == query.AccountId.ToString())))
            .ReturnsAsync(accountResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.HasDirectTransfersAvailable.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task ThenHasReservationsAvailable_WhenLimitNotReached(
        GetSelectFundingOptionsQuery query,
        GetAccountReservationsStatusResponse reservationsResponse,
        GetAccountResponse accountResponse,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsApiClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        reservationsResponse.HasReachedReservationsLimit = false;

        reservationsApiClient.Setup(x =>
                x.Get<GetAccountReservationsStatusResponse>(
                    It.Is<GetAccountReservationsStatusRequest>(x =>
                        x.AccountId == query.AccountId && x.TransferSenderId == null)))
            .ReturnsAsync(reservationsResponse);

        financeApiClient.Setup(x =>
                x.Get<GetTransferConnectionsResponse>(
                    It.Is<GetTransferConnectionsRequest>(x => x.AccountId == query.AccountId)))
            .ReturnsAsync(new GetTransferConnectionsResponse());

        accountsApiClient.Setup(x =>
                x.Get<GetAccountResponse>(
                    It.Is<GetAccountRequest>(x => x.HashedAccountId == query.AccountId.ToString())))
            .ReturnsAsync(accountResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.HasAdditionalReservationFundsAvailable.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task ThenHasUnallocatedReservations_WhenPendingReservationsExist(
        GetSelectFundingOptionsQuery query,
        GetAccountReservationsStatusResponse reservationsResponse,
        GetAccountResponse accountResponse,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsApiClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        reservationsResponse.HasPendingReservations = true;

        reservationsApiClient.Setup(x =>
                x.Get<GetAccountReservationsStatusResponse>(
                    It.Is<GetAccountReservationsStatusRequest>(x =>
                        x.AccountId == query.AccountId && x.TransferSenderId == null)))
            .ReturnsAsync(reservationsResponse);

        financeApiClient.Setup(x =>
                x.Get<GetTransferConnectionsResponse>(
                    It.Is<GetTransferConnectionsRequest>(x => x.AccountId == query.AccountId)))
            .ReturnsAsync(new GetTransferConnectionsResponse());

        accountsApiClient.Setup(x =>
                x.Get<GetAccountResponse>(
                    It.Is<GetAccountRequest>(x => x.HashedAccountId == query.AccountId.ToString())))
            .ReturnsAsync(accountResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.HasUnallocatedReservationsAvailable.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task ThenIsLevyAccount_WhenEmploymentTypeIsLevy(
        GetSelectFundingOptionsQuery query,
        GetAccountReservationsStatusResponse reservationsResponse,
        GetAccountResponse accountResponse,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsApiClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        accountResponse.ApprenticeshipEmployerType = "LEVY";

        reservationsApiClient.Setup(x =>
                x.Get<GetAccountReservationsStatusResponse>(
                    It.Is<GetAccountReservationsStatusRequest>(x =>
                        x.AccountId == query.AccountId && x.TransferSenderId == null)))
            .ReturnsAsync(reservationsResponse);

        financeApiClient.Setup(x =>
                x.Get<GetTransferConnectionsResponse>(
                    It.Is<GetTransferConnectionsRequest>(x => x.AccountId == query.AccountId)))
            .ReturnsAsync(new GetTransferConnectionsResponse());

        accountsApiClient.Setup(x =>
                x.Get<GetAccountResponse>(
                    It.Is<GetAccountRequest>(x => x.HashedAccountId == query.AccountId.ToString())))
            .ReturnsAsync(accountResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.IsLevyAccount.Should().BeTrue();
    }
}