using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Api.UnitTests.Models
{
    public class WhenCastingGetStandardDetailsResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            StandardDetailResponse source)
        {
            var response = (GetStandardDetailsResponse)source;

            response.Should().BeEquivalentTo(source);
        }
    }
}