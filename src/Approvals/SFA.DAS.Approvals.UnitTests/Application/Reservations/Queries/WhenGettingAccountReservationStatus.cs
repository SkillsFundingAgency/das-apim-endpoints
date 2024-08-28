using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Reservations.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Reservations.Queries;

public class WhenGettingAccountReservationStatus
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Returns_Status_And_Unallocated_Count(
        GetAccountReservationsStatusQuery query,
        GetAccountReservationsStatusResponse apiStatusResponse,
        IEnumerable<GetAccountReservationItem> apiReservations,
        [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> apiClient,
        GetAccountReservationsStatusQueryHandler handler
    )
    {
        apiClient
            .Setup(x => x.Get<GetAccountReservationsStatusResponse>(It.IsAny<GetAccountReservationsStatusRequest>()))
            .ReturnsAsync(apiStatusResponse);

        apiClient
            .Setup(x => x.GetAll<GetAccountReservationItem>(It.IsAny<GetAccountReservationsRequest>()))
            .ReturnsAsync(apiReservations);


        var actual = await handler.Handle(query, CancellationToken.None);

        actual.CanAutoCreateReservations.Should().Be(apiStatusResponse.CanAutoCreateReservations);
        actual.HasReachedReservationsLimit.Should().Be(apiStatusResponse.HasReachedReservationsLimit);
        actual.UnallocatedPendingReservations.Should().Be(apiReservations.Count(x => !x.IsExpired && x.Status == ReservationStatus.Pending));
    }
}