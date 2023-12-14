using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetReservations;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries
{
    public class WhenHandlingTheGetReservationsRequestQuery
    {
        [Test]
        [MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Reservations_Returned(
            GetReservationsQuery query,
            IList<GetReservationsResponseListItem> apiResponse,
            [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> apiClient,
            GetReservationsQueryHandler handler
        )
        {
            apiClient.Setup(x => x.GetAll<GetReservationsResponseListItem>(It.IsAny<GetReservationsRequest>()))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Reservations.Should().BeEquivalentTo(apiResponse);
        }
    }
}