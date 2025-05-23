using System.Collections.Generic;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.UnitTests.Extensions
{
    [TestFixture]
    public class EmailTemplateAddressExtensionTests
    {
        [Test]
        public void GetEmploymentLocations_ShouldReturnEmptyString_WhenAddressesIsEmpty()
        {
            // Arrange
            var addresses = new List<Address>();

            // Act
            var result = EmailTemplateAddressExtension.GetEmploymentLocations(addresses);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void GetEmploymentLocations_ShouldReturnCorrectString_WhenAddressesIsNotEmpty()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { AddressLine1 = "123 Main St", Postcode = "12345" },
                new() { AddressLine1 = "456 Elm St", Postcode = "67890" }
            };

            // Act
            var result = EmailTemplateAddressExtension.GetEmploymentLocations(addresses);

            // Assert
            result.Should().Be("123 Main St (12345) and 1 other available locations");
        }

        [Test]
        public void GetEmploymentLocations_ShouldReturnCorrectString_With_Same_Cities()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { AddressLine1 = "Coventry", Postcode = "CV2" },
                new() { AddressLine1 = "Leeds", Postcode = "LS6" },
                new() { AddressLine1 = "Leeds", Postcode = "LS16" },
                new() { AddressLine1 = "Leeds", Postcode = "LS9" },
                new() { AddressLine1 = "London", Postcode = "SW1A" },
                new() { AddressLine1 = "Newcastle", Postcode = "ST5" },
            };

            // Act
            var result = EmailTemplateAddressExtension.GetEmploymentLocations(addresses);

            // Assert
            result.Should().Be("Coventry (CV2) and 5 other available locations");
        }

        [Test]
        public void GetEmploymentLocations_ShouldReturnCorrectString_With_Same_Not_Alphabetical_Cities()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { AddressLine1 = "Newcastle", Postcode = "ST5" },
                new() { AddressLine1 = "Coventry", Postcode = "CV2" },
                new() { AddressLine1 = "Leeds", Postcode = "LS6" },
                new() { AddressLine1 = "Leeds", Postcode = "LS16" },
                new() { AddressLine1 = "Leeds", Postcode = "LS9" },
                new() { AddressLine1 = "London", Postcode = "SW1A" },
            };

            // Act
            var result = EmailTemplateAddressExtension.GetEmploymentLocations(addresses);

            // Assert
            result.Should().Be("Newcastle (ST5) and 5 other available locations");
        }

        [Test]
        public void GetEmploymentLocations_ShouldReturnCorrectString_With_Same_Cities_With_Different_Postcodes()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { AddressLine1 = "Leeds", Postcode = "LS6" },
                new() { AddressLine1 = "Leeds", Postcode = "LS16" },
                new() { AddressLine1 = "Leeds", Postcode = "LS9" },
            };

            // Act
            var result = EmailTemplateAddressExtension.GetEmploymentLocations(addresses);

            // Assert
            result.Should().Be("Leeds (LS6) and 2 other available locations");
        }

        [Test]
        public void GetOneLocationCityName_ShouldReturnEmptyString_WhenAddressIsNull()
        {
            // Act
            var result = EmailTemplateAddressExtension.GetOneLocationCityName(null);

            // Assert
            result.Should().BeNullOrEmpty();
        }

        [Test]
        public void GetOneLocationCityName_ShouldReturnPostcode_And_City()
        {
            // Arrange
            var address = new Address { AddressLine1 = "123 Main St", Postcode = "12345" };

            // Act
            var result = EmailTemplateAddressExtension.GetOneLocationCityName(address);

            // Assert
            result.Should().Be("123 Main St (12345)");
        }

        [Test]
        public void GetOneLocationCityName_ShouldReturnPostcode_WhenCityIsEmpty()
        {
            // Arrange
            var address = new Address { Postcode = "12345" };

            // Act
            var result = EmailTemplateAddressExtension.GetOneLocationCityName(address);

            // Assert
            result.Should().Be("12345");
        }

        [Test]
        public void GetOneLocationCityName_ShouldReturnCityAndPostcode_WhenCityIsNotEmpty()
        {
            // Arrange
            var address = new Address { AddressLine1 = "123 Main St", AddressLine2 = "City", Postcode = "12345" };

            // Act
            var result = EmailTemplateAddressExtension.GetOneLocationCityName(address);

            // Assert
            result.Should().Be("City (12345)");
        }

        [Test]
        public void GetEmploymentLocationCityNames_ShouldReturnCorrectString_WhenAddressesIsNotEmpty()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { AddressLine1 = "123 Main St", AddressLine2 = "CityA", Postcode = "12345" },
                new() { AddressLine1 = "456 Elm St", AddressLine2 = "CityB", Postcode = "67890" },
                new() { AddressLine1 = "789 Oak St", AddressLine2 = "CityA", Postcode = "54321" }
            };

            // Act
            var result = EmailTemplateAddressExtension.GetEmploymentLocationCityNames(addresses);

            // Assert
            result.Should().Be("CityA, CityB");
        }

        [Test]
        public void GetEmploymentLocationCityNames_ShouldReturnCorrectString_WhenAddressesIsNotEmpty_Not_In_Different_Ordering()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { AddressLine1 = "456 Elm St", AddressLine2 = "CityB", Postcode = "67890" },
                new() { AddressLine1 = "123 Main St", AddressLine2 = "CityA", Postcode = "12345" },
                new() { AddressLine1 = "789 Oak St", AddressLine2 = "CityA", Postcode = "54321" }
            };

            // Act
            var result = EmailTemplateAddressExtension.GetEmploymentLocationCityNames(addresses);

            // Assert
            result.Should().Be("CityA, CityB");
        }

        [Test]
        public void GetEmploymentLocationCityNames_ShouldReturnCorrectString_WhenAddressesIsNotEmpty_Not_In_Different_Postcode()
        {
            // Arrange
            var addresses = new List<Address>
            {
                new() { AddressLine1 = "456 Elm St", AddressLine2 = "CityA", Postcode = "67890" },
                new() { AddressLine1 = "123 Main St", AddressLine2 = "CityA", Postcode = "67890" },
                new() { AddressLine1 = "789 Oak St", AddressLine2 = "CityA", Postcode = "67890" }
            };

            // Act
            var result = EmailTemplateAddressExtension.GetEmploymentLocationCityNames(addresses);

            // Assert
            result.Should().Be("CityA (3 available locations)");
        }
    }
}
