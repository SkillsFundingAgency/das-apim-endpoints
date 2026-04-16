using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Infrastructure.Ukrlp;

namespace SFA.DAS.SharedOuterApi.UnitTests.Infrastructure.Ukrlp
{
    [TestFixture]
    public class ProviderAddressTests
    {
        [Test, AutoData]
        public void Operator_ConvertsToProviderAddress(Provider source)
        {
            var providerAddress = (ProviderAddress)source;

            providerAddress.Should().BeEquivalentTo(source, option =>
            {
                option.ExcludingMissingMembers(); 
                return option;
            });
        }
    }
}
