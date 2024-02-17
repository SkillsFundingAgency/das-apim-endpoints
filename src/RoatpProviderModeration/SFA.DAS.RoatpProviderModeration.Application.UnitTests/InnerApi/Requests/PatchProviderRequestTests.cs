using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;
using System.Linq;
using System.Web;

namespace SFA.DAS.RoatpProviderModeration.Application.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class PatchProviderRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(ProviderUpdateModel data)
        {
            var request = new PatchProviderRequest(data);
            Assert.That(request.Data.Count, Is.EqualTo(1));
            Assert.That(request.Data.Any(x => x.Path == "MarketingInfo"),Is.True);
            request.PatchUrl.Should().Be($"providers/{data.Ukprn}?userId={HttpUtility.UrlEncode(data.UserId)}&userDisplayName={HttpUtility.UrlEncode(data.UserDisplayName)}");
        }
      
        [TestCase("TestDescription", 1)]
        public void Request_BuildDataPatchFromModel(string marketingInfo, int numberOfPatches)
        {
            var model = new ProviderUpdateModel
            {
                MarketingInfo = marketingInfo,
            };
            var request = new PatchProviderRequest(model);

            Assert.That(request.Data.Count, Is.EqualTo(numberOfPatches));
            Assert.That(request.Data.Any(x => x.Path == "MarketingInfo"), Is.EqualTo(marketingInfo != null));
        }
    }
}
