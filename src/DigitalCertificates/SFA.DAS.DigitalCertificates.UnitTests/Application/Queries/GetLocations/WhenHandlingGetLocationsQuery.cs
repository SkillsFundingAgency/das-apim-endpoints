using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Application.Queries.GetLocations;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetLocations
{
    public class WhenHandlingGetLocationsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Addresses_Are_Retrieved_Successfully(
            GetLocationsQuery query,
            GetAddressesListResponse responseBody,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            GetLocationsQueryHandler handler)
        {
            var apiResponse = new ApiResponse<GetAddressesListResponse>(responseBody, HttpStatusCode.OK, string.Empty);

            mockLocationApiClient
                .Setup(c => c.GetWithResponseCode<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.Addresses.Should().BeEquivalentTo(responseBody);
        }

        [Test, MoqAutoData]
        public async Task Then_NotFound_Returns_Null(
            GetLocationsQuery query,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            GetLocationsQueryHandler handler)
        {
            var apiResponse = new ApiResponse<GetAddressesListResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockLocationApiClient
                .Setup(c => c.GetWithResponseCode<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
                .ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.Addresses.Should().BeNull();
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            GetLocationsQuery query,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockLocationApiClient,
            GetLocationsQueryHandler handler)
        {
            mockLocationApiClient
                .Setup(c => c.GetWithResponseCode<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
                .ThrowsAsync(new Exception("Bad request"));

            Func<Task> act = async () => await handler.Handle(query, CancellationToken.None);

            act.Should().ThrowAsync<Exception>();
        }
    }
}
