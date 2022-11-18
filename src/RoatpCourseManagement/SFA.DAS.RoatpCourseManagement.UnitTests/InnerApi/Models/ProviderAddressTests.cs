using AutoFixture.NUnit3;
using NUnit.Framework;
using FluentAssertions;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models.Ukrlp;
using SFA.DAS.RoatpCourseManagement.Application.UkrlpData;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Models
{

    [TestFixture]
    public class ProviderAddressTests
    {
        private const string LegalIdentifier = "L";

        [Test, AutoData]
        public void Operator_ConvertsToProviderAddress(Provider source)
        {
            source.ProviderContacts[0].ContactType = LegalIdentifier;

            var providerAddress = (ProviderAddress)source;
            providerAddress.ProviderName.Should().Be(source.ProviderName);
            providerAddress.Ukprn.Should().Be(source.UnitedKingdomProviderReferenceNumber);
            providerAddress.Address1.Should().Be(source.ProviderContacts[0].ContactAddress.Address1);
            providerAddress.Address2.Should().Be(source.ProviderContacts[0].ContactAddress.Address2);
            providerAddress.Address3.Should().Be(source.ProviderContacts[0].ContactAddress.Address3);
            providerAddress.Address4.Should().Be(source.ProviderContacts[0].ContactAddress.Address4);
            providerAddress.Town.Should().Be(source.ProviderContacts[0].ContactAddress.Town);
            providerAddress.Postcode.Should().Be(source.ProviderContacts[0].ContactAddress.PostCode);
        }
    }
}
