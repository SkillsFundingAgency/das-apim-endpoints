using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class GetProviderAddressTests
    {
        [Test, AutoData]
        public void WhenMappingGetProviderStandardItemAddress_ThenMapsTheFields(GetProviderStandardItemAddress source)
        {
            var actual = new GetProviderAddress().Map(source, true);

            actual.Should().BeEquivalentTo(source);
        }

        [Test, AutoData]
        public void WhenMappingGetProviderStandardItemAddress_AndNoLocation_ThenSetsDistanceToNull(GetProviderStandardItemAddress source)
        {
            var actual = new GetProviderAddress().Map(source, false);

            actual.DistanceInMiles.Should().BeNull();
        }
    }
}