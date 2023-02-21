using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Locations.Queries.GetAddresses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Locations.Queries.GetAddresses
{
    public class GetAddressesQueryHandlerTests
    {
        [Test]
        [MoqAutoData]
        public async Task Handle_ReturnAddressesBasedOnQuery(
            GetAddressesQuery query,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
            [Frozen(Matching.ImplementedInterfaces)]
            GetAddressesQueryHandler handler,
            GetAddressesQueryResult response)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetAddressesQueryResult>(It.IsAny<GetAddressesQueryRequest>()))
                .ReturnsAsync(new ApiResponse<GetAddressesQueryResult>(response, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result?.AddressesResponse?.Addresses.ToList().Count.Should().NotBe(0);
            result?.AddressesResponse?.Addresses.ToList().Count.Should().Be(response.AddressesResponse.Addresses.ToList().Count);
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_AddressesNotFoundOrError(
            GetAddressesQuery query,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
            [Frozen(Matching.ImplementedInterfaces)]
            GetAddressesQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.GetWithResponseCode<GetAddressesQueryResult>(It.IsAny<GetAddressesQueryRequest>()))
                .ReturnsAsync(new ApiResponse<GetAddressesQueryResult>(null!, HttpStatusCode.BadRequest, string.Empty));

            var result = await handler.Handle(query, CancellationToken.None);

            result?.AddressesResponse?.Should().BeNull();
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_EmptyQueryThrowsError(
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
            [Frozen(Matching.ImplementedInterfaces)]
            GetAddressesQueryHandler handler)
        {
            var query = new GetAddressesQuery(string.Empty);

            Func<Task> action = () => handler.Handle(query, CancellationToken.None);

            await action.Should().ThrowAsync<ArgumentException>("Query is required (Parameter 'Query')");
        }

        [Test]
        [MoqAutoData]
        public async Task Handle_NullQueryThrowsError(
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> apiClient,
            [Frozen(Matching.ImplementedInterfaces)]
            GetAddressesQueryHandler handler)
        {
            var query = new GetAddressesQuery(null!);

            Func<Task> action = () => handler.Handle(query, CancellationToken.None);

            await action.Should().ThrowAsync<ArgumentException>("Query is required (Parameter 'Query')");
        }
    }
}