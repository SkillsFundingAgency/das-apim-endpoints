using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Reservations;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAvailableDatesRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(long accountLegalEntityId)
        {
            var actual = new GetAvailableDatesRequest(accountLegalEntityId);

            actual.GetUrl.Should().Be($"api/rules/available-dates/{accountLegalEntityId}");
        }
    }
}
