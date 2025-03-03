using FluentAssertions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.UnitTests.Extensions;

public class WhenAnonymisingAnAddress
{
    [TestCase("AddressLine1", "AddressLine2", "AddressLine3", "AddressLine4", "SW1A 2AA", "AddressLine4", "SW1A")]
    [TestCase("AddressLine1", null, null, null, "SW1A 2AA", null, "SW1A")]
    [TestCase("AddressLine1", "AddressLine2", null, null, "B1", "AddressLine2", "B1")]
    [TestCase(null, null, null, null, null, null, null)]
    [TestCase(null, null, null, null, "", null, "")]
    [TestCase(null, null, null, "", null, null, null)]
    public void Then_Address_Is_Anonymised_Correctly(string addressLine1, string addressLine2, string addressLine3, string addressLine4, string postcode, string expectedAddressLine3, string expectedPostcode)
    {
        // arrange
        var address = new Address
        {
            AddressLine1 = addressLine1,
            AddressLine2 = addressLine2,
            AddressLine3 = addressLine3,
            AddressLine4 = addressLine4,
            Postcode = postcode,
            Latitude = 1,
            Longitude = 2
        };
        
        // act
        address.Anonymise();
        
        // assert
        address.AddressLine1.Should().BeNull();
        address.AddressLine2.Should().BeNull();
        address.AddressLine3.Should().Be(expectedAddressLine3);
        address.AddressLine4.Should().BeNull();
        address.Postcode.Should().Be(expectedPostcode);
        address.Latitude.Should().BeNull();
        address.Longitude.Should().BeNull();
    }
}