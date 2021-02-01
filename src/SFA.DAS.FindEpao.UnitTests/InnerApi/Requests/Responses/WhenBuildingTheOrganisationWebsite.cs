using NUnit.Framework;
using SFA.DAS.FindEpao.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindEpao.UnitTests.InnerApi.Requests.Responses
{
    public class WhenBuildingTheOrganisationWebsite
    {
        [Test]
        [MoqInlineAutoData(null, null)]
        [MoqInlineAutoData("", "")]
        [MoqInlineAutoData("https://www.test.com", "https://www.test.com")]
        [MoqInlineAutoData("http://www.test.com", "http://www.test.com")]
        [MoqInlineAutoData("Http://www.test.com", "Http://www.test.com")]
        [MoqInlineAutoData("www.test.com", "https://www.test.com")]
        public void Then_The_Https_Protocal_Is_Added_If_None_Is_Found(string websiteLink, string expectedWebsiteLink)
        {
            var organisationData = new GetEpaoOrganisationData{WebsiteLink = websiteLink};

            Assert.AreEqual(expectedWebsiteLink, organisationData.WebsiteLink);
        }
    }
}