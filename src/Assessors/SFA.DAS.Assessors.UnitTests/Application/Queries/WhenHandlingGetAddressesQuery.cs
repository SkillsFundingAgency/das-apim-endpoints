﻿using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Assessors.Application.Queries.GetAddresses;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Assessors.UnitTests.Application.Queries
{
    public class WhenHandlingGetAddressesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Addresses_From_Locations_Api(
            GetAddressesQuery query,
            GetAddressesListResponse apiResponse,
            [Frozen] Mock<ILocationApiClient<LocationApiConfiguration>> mockApiClient,
            GetAddressesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetAddressesListResponse>(It.IsAny<GetAddressesQueryRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AddressesResponse.Addresses.Should().BeEquivalentTo(apiResponse.Addresses);
        }
    }
}
