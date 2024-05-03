using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.Rules.Queries.GetAvailableDates;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.Rules
{
    public class WhenHandlingGetAvailableDatesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Cohort_From_CommitmentsV2_Api(
           GetAvailableDatesQuery query,
           GetAvailableDatesResponse apiResponse,
           [Frozen] Mock<IReservationApiClient<ReservationApiConfiguration>> mockApiClient,
           GetAvailableDatesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetAvailableDatesResponse>(It.Is<GetAvailableDatesRequest>(
                    request => request.AccountLegalEntityId == query.AccountLegalEntityId
                    )))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Should().BeEquivalentTo(apiResponse);
        }
    }
}
