using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.EmployerAan.Application.Onboarding.NotificationsLocations;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Onboarding.NotificationLocations
{
    [TestFixture]
    public class GetNotificationsLocationsQueryHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_ReturnAddressesBasedOnQuery(GetNotificationsLocationsQuery query,
           [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
           [Frozen] GetNotificationsLocationsQueryHandler handler,
           GetLocationsListResponse apiResponse)
        {
            var expectedApiRequest = new GetLocationsQueryRequest(query.SearchTerm);

            apiClient
                .Setup(x => x.Get<GetLocationsListResponse>(It.Is<GetLocationsQueryRequest>(r => r.GetUrl == expectedApiRequest.GetUrl)))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.Locations.Should().BeEquivalentTo(apiResponse.Locations.Select(x => new GetNotificationsLocationsQueryResult.Location
            {
                Name = x.DisplayName,
                GeoPoint = x.Location.GeoPoint
            }).Take(GetNotificationsLocationsQueryHandler.MaxResults));
        }
    }
}
