using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Models;

namespace SFA.DAS.Reservations.Api.UnitTests.Models
{
    public class WhenCastingGetProviderResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            InnerApi.Responses.GetProviderResponse source)
        {
            var response = (GetProviderResponse)source;

            response.Should().BeEquivalentTo(source);
        }

        [Test]
        public void Then_Maps_Null_If_Null_Source()
        {
            var response = (GetProviderResponse) (InnerApi.Responses.GetProviderResponse)null;

            response.Should().BeNull();
        }
    }
}