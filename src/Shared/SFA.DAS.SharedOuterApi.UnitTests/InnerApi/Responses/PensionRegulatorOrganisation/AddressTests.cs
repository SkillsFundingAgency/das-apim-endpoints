using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Responses.PensionRegulatorOrganisation;
public class AddressTests
{
    [Test]
    public void ToString_ReturnCommaDelimitedAddress()
    {
        Address address = new()
        {
            Line1 = "Line1",
            Line2 = "Line2",
            Line3 = " ",
            Line5 = "Line5",
            Postcode = "POst coDE"
        };

        address.ToString().Should().Be("Line1, Line2, Line5, POst coDE");
    }
}
