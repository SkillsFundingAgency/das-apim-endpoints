using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Requests;

namespace SFA.DAS.Campaign.UnitTests.ExternalApi.Requests
{
    public class WhenBuildingGetPanelRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(int panelId)
        {
            var actual = new GetPanelRequest(panelId);

            actual.GetUrl.Should().Be($"entries?content_type=panel&fields.id={panelId}");
        }
    }
}
