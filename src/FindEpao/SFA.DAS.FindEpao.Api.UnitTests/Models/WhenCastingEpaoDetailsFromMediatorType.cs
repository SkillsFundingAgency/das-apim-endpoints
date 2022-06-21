using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Api.UnitTests.Models
{
    public class WhenCastingEpaoDetailsFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetEpaoResponse source)
        {
            var response = (Api.Models.EpaoDetails)source;

            response.Should().BeEquivalentTo(source);
        }
    }
}