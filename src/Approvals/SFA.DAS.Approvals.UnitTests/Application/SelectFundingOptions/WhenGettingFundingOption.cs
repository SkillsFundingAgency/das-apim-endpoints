using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.SelectFunding.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFinance;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.LevyTransferMatching;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.SelectFundingOptions;

public class WhenGettingFundingOption
{
    [Test, MoqAutoData]
    public async Task ThenDirectTransfersAreAvailable_WhenTransferConnectionsExist(
        GetSelectFundingOptionsQuery query,
        IEnumerable<GetTransferConnectionsResponse.TransferConnection> directTransfersResponse,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        financeApiClient.Setup(x =>
                x.Get<IEnumerable<GetTransferConnectionsResponse.TransferConnection>>(
                    It.Is<GetTransferConnectionsRequest>(x => x.AccountId == query.AccountId)))
            .ReturnsAsync(directTransfersResponse);
        
        var actual = await handler.Handle(query, CancellationToken.None);

        actual.HasDirectTransfersAvailable.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task ThenHasReservationsAvailable_WhenLimitNotReached(
        GetSelectFundingOptionsQuery query,
        GetAccountReservationsStatusResponse reservationsResponse,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        reservationsResponse.HasReachedReservationsLimit = false;

        reservationsApiClient.Setup(x =>
                x.Get<GetAccountReservationsStatusResponse>(
                    It.Is<GetAccountReservationsStatusRequest>(x =>
                        x.AccountId == query.AccountId && x.TransferSenderId == null)))
            .ReturnsAsync(reservationsResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.HasAdditionalReservationFundsAvailable.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task ThenHasUnallocatedReservations_WhenPendingReservationsExist(
        GetSelectFundingOptionsQuery query,
        GetAccountReservationsStatusResponse reservationsResponse,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        reservationsResponse.HasPendingReservations = true;

        reservationsApiClient.Setup(x =>
                x.Get<GetAccountReservationsStatusResponse>(
                    It.Is<GetAccountReservationsStatusRequest>(x =>
                        x.AccountId == query.AccountId && x.TransferSenderId == null)))
            .ReturnsAsync(reservationsResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.HasUnallocatedReservationsAvailable.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task ThenIsLevyAccount_WhenEmploymentTypeIsLevy(
        GetSelectFundingOptionsQuery query,
        GetAccountResponse accountResponse,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        accountResponse.ApprenticeshipEmployerType = "LEVY";

        accountsApiClient.Setup(x =>
                x.Get<GetAccountResponse>(
                    It.Is<GetAccountRequest>(x => x.HashedAccountId == query.AccountId.ToString())))
            .ReturnsAsync(accountResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.IsLevyAccount.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task ThenHasLtmTransfers_WhenApprovedApplicationsExist(
        GetSelectFundingOptionsQuery query,
        GetApplicationsResponse ltmTransfersResponse,
        [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> ltmApiClient,
        GetSelectFundingOptionsQueryHandler handler
    )
    {
        ltmApiClient.Setup(x =>
                x.Get<GetApplicationsResponse>(
                    It.Is<GetAcceptedEmployerAccountPledgeApplicationsRequest>(x => x.EmployerAccountId == query.AccountId)))
            .ReturnsAsync(ltmTransfersResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.HasLtmTransfersAvailable.Should().BeTrue();
    }

}