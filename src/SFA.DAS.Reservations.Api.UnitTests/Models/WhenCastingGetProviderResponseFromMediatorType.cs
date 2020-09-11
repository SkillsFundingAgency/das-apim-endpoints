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
    }
}