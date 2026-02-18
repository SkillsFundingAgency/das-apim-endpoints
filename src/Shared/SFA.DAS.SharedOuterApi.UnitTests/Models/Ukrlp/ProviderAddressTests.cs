using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Ukrlp;
using SFA.DAS.SharedOuterApi.Models.Ukrlp;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.UkrlpData
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
