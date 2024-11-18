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
    public async Task Then_The_Api_To_GetSelectFundingOptions_Returns_ExpectedValues(
        GetSelectFundingOptionsQuery query,
        GetAccountReservationsStatusResponse reservationsResponse,
        GetTransferConnectionsResponse directTransfersResponse,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> reservationsApiClient,
        [Frozen] Mock<IFinanceApiClient<FinanceApiConfiguration>> financeApiClient,
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


        var actual = await handler.Handle(query, CancellationToken.None);

        actual.HasDirectTransfersAvailable.Should().Be(directTransfersResponse.TransferConnections.Any());
        actual.IsLevyAccount.Should().Be(reservationsResponse.CanAutoCreateReservations);
        actual.HasAdditionalReservationFundsAvailable.Should().Be(!reservationsResponse.HasReachedReservationsLimit);
        actual.HasUnallocatedReservationsAvailable.Should().Be(reservationsResponse.HasPendingReservations);
    }
}