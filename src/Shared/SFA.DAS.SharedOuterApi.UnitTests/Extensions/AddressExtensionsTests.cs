using FluentAssertions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;

namespace SFA.DAS.SharedOuterApi.UnitTests.Extensions
{
    [TestFixture]
    public class AddressExtensionsTests
    {
        [Test]
        public void OrderByCity_ShouldReturnAddressesOrderedByCityFields()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { AddressLine1 = "Line1", AddressLine2 = "Line2", AddressLine3 = "CityB", AddressLine4 = "RegionA" },
                new() { AddressLine1 = "Line1", AddressLine2 = "Line2", AddressLine3 = "CityA", AddressLine4 = "RegionB" },
                new() { AddressLine1 = "Line1", AddressLine2 = "Line2", AddressLine3 = "CityC", AddressLine4 = "RegionA" }
            };

            // Act
            var result = addresses.OrderByCity();

            // Assert
            result.Should().BeInAscendingOrder(x => x.AddressLine4)
                .And.ThenBeInAscendingOrder(x => x.AddressLine3)
                .And.ThenBeInAscendingOrder(x => x.AddressLine2)
                .And.ThenBeInAscendingOrder(x => x.AddressLine1);
        }

        [Test]
        public void OrderByCity_ShouldReturnEmptyList_WhenInputIsEmpty()
        {
            // Arrange
            var addresses = new List<Address>();

            // Act
            var result = addresses.OrderByCity();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
