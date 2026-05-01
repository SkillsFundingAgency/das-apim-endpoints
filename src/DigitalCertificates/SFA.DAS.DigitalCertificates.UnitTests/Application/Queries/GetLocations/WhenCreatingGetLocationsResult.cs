using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetLocations;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetLocations
{
    public class WhenCreatingGetLocationsResult
    {
        [Test, MoqAutoData]
        public void Then_Addresses_Property_Can_Be_Set(GetAddressesListResponse addresses)
        {
            var result = new GetLocationsResult { Addresses = addresses };

            result.Addresses.Should().BeEquivalentTo(addresses);
        }
    }
}
