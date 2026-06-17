using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.Roatp;

public class GetUkrlpProvidersRequestTests
{
    [Test]
    public void GetUrl_ReturnsExpectedUrl()
    {
        // Arrange
        var sut = new GetUkrlpProvidersRequest
        {
            Ukprns = new List<int> { 12345678, 87654321 },
            UpdatedSinceDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };
        // Act
        var url = sut.GetUrl;
        // Assert
        Assert.That(url, Is.EqualTo("/ukrlp/providers?ukprns=12345678&ukprns=87654321&updatedSince=2024-01-01"));
    }

    [Test]
    public void GetUrl_HandlesEmptyUkprns()
    {
        // Arrange
        var sut = new GetUkrlpProvidersRequest
        {
            Ukprns = new List<int>(),
            UpdatedSinceDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Unspecified)
        };
        // Act
        var url = sut.GetUrl;
        // Assert
        Assert.That(url, Is.EqualTo("/ukrlp/providers?updatedSince=2024-01-01"));
    }

    [Test]
    public void GetUrl_HandlesNullUpdatedSinceDate()
    {
        // Arrange
        var sut = new GetUkrlpProvidersRequest
        {
            Ukprns = new List<int> { 12345678, 87654321 },
            UpdatedSinceDate = null
        };
        // Act
        var url = sut.GetUrl;
        // Assert
        Assert.That(url, Is.EqualTo("/ukrlp/providers?ukprns=12345678&ukprns=87654321"));
    }

    [Test]
    public void GetUrl_HandlesEmptyUkprnsAndNullUpdatedSinceDate()
    {
        // Arrange
        var sut = new GetUkrlpProvidersRequest
        {
            Ukprns = new List<int>(),
            UpdatedSinceDate = null
        };
        // Act
        var url = sut.GetUrl;
        // Assert
        Assert.That(url, Is.EqualTo("/ukrlp/providers?"));
    }
}
