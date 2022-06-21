using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetProviderStandardItemAddressToGetProviderAddress
    {
        [Test, AutoData]
        public void Then_Maps_The_Fields(GetProviderStandardItemAddress source)
        {
            var actual = new GetProviderAddress().Map(source, true);
            
            actual.Should().BeEquivalentTo(source);
        }

        [Test, AutoData]
        public void Then_No_Location_Sets_Distance_To_Null(GetProviderStandardItemAddress source)
        {
            var actual = new GetProviderAddress().Map(source, false);

            actual.DistanceInMiles.Should().BeNull();
        }
    }
}