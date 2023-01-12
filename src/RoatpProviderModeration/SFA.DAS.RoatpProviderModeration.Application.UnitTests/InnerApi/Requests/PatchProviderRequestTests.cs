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
            Assert.AreEqual(1, request.Data.Count);
            Assert.IsTrue(request.Data.Any(x => x.Path == "MarketingInfo"));
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

            Assert.AreEqual(numberOfPatches, request.Data.Count);
            Assert.AreEqual(request.Data.Any(x => x.Path == "MarketingInfo"), marketingInfo != null);
        }
    }
}
