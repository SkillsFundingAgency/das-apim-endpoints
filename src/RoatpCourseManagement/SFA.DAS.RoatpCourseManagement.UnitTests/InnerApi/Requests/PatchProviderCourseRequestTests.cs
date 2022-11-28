using System.Linq;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class PatchProviderCourseRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(ProviderCourseUpdateModel data)
        {
            var request = new PatchProviderCourseRequest(data);
            Assert.AreEqual(5, request.Data.Count);
            Assert.IsTrue(request.Data.Any(x => x.Path == "ContactUsEmail"));
            Assert.IsTrue(request.Data.Any(x => x.Path == "ContactUsPageUrl"));
            Assert.IsTrue(request.Data.Any(x => x.Path == "ContactUsPhoneNumber"));
            Assert.IsTrue(request.Data.Any(x => x.Path == "StandardInfoUrl"));
            Assert.IsTrue(request.Data.Any(x => x.Path == "IsApprovedByRegulator"));
            request.PatchUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}?userId={HttpUtility.UrlEncode(data.UserId)}&userDisplayName={HttpUtility.UrlEncode(data.UserDisplayName)}");
        }

        [TestCase("test@test.com", "1234567890", "http://www.google.com/contact.us", "http://www.google.com", true, 5)]
        [TestCase("test@test.com","1234567890","http://www.google.com/contact.us","http://www.google.com",null,4)]
        [TestCase(null, "1234567890", "http://www.google.com/contact.us", "http://www.google.com", true, 4)]
        [TestCase("test@test.com", null, "http://www.google.com/contact.us", "http://www.google.com", true, 4)]
        [TestCase("test@test.com", "1234567890", null, "http://www.google.com", true, 4)]
        [TestCase("test@test.com", "1234567890", "http://www.google.com/contact.us", null, true, 4)]
        [TestCase("test@test.com", "1234567890", "http://www.google.com/contact.us", null, null, 3)]
        [TestCase("test@test.com", "1234567890", null, "http://www.google.com", null, 3)]
        [TestCase("test@test.com", null, "http://www.google.com/contact.us", "http://www.google.com", null, 3)]
        [TestCase(null, "1234567890", "http://www.google.com/contact.us", "http://www.google.com", null, 3)]
        [TestCase(null, null, "http://www.google.com/contact.us", "http://www.google.com", true, 3)]
        [TestCase(null, "1234567890", null, "http://www.google.com", true, 3)]
        [TestCase(null, "1234567890", "http://www.google.com/contact.us", null, true, 3)]
        [TestCase("test@test.com", null,null, "http://www.google.com", true, 3)]
        [TestCase("test@test.com", null, "http://www.google.com/contact.us", null, true, 3)]
        [TestCase("test@test.com", "1234567890", null, null, true, 3)]
        [TestCase(null, null, "http://www.google.com/contact.us", "http://www.google.com", null, 2)]
        [TestCase(null, "1234567890", null, "http://www.google.com", null, 2)]
        [TestCase(null, "1234567890", "http://www.google.com/contact.us", null, null, 2)]
        [TestCase("test@test.com", null, null, "http://www.google.com", null, 2)]
        [TestCase("test@test.com", null, "http://www.google.com/contact.us", null, null, 2)]
        [TestCase("test@test.com", "1234567890", null, null, null, 2)]
        [TestCase(null, null, null, "http://www.google.com", true, 2)]
        [TestCase(null, null, "http://www.google.com/contact.us", null, true, 2)]
        [TestCase(null, "1234567890", null, null, true, 2)]
        [TestCase("test@test.com", null, null, null, true, 2)]
        [TestCase("test@test.com", null, null, null, null, 1)] 
        [TestCase(null, "1234567890", null, null, null, 1)] 
        [TestCase(null, null, "http://www.google.com/contact.us", null, null, 1)]
        [TestCase(null, null, null, "http://www.google.com", null, 1)]
        [TestCase(null, null, null, null, true, 1)]
        public void Request_BuildDataPatchFromModel( string contactUsEmail, string contactUsPhoneNumber, string contactUsPageUrl, string standardInfoUrl, bool? isApprovedByRegulator, int numberOfPatches)
        {
            var model = new ProviderCourseUpdateModel
            {
                ContactUsEmail = contactUsEmail,
                ContactUsPageUrl = contactUsPageUrl,
                ContactUsPhoneNumber = contactUsPhoneNumber,
                StandardInfoUrl = standardInfoUrl,
                IsApprovedByRegulator = isApprovedByRegulator
            };
            var request = new PatchProviderCourseRequest(model);

             Assert.AreEqual(numberOfPatches, request.Data.Count);
             Assert.AreEqual(request.Data.Any(x => x.Path == "ContactUsEmail"), contactUsEmail!=null);
             Assert.AreEqual(request.Data.Any(x => x.Path == "ContactUsPageUrl"), contactUsPageUrl != null);
             Assert.AreEqual(request.Data.Any(x => x.Path == "StandardInfoUrl"), standardInfoUrl != null);
             Assert.AreEqual(request.Data.Any(x => x.Path == "ContactUsPhoneNumber"), contactUsPhoneNumber != null);
             Assert.AreEqual(request.Data.Any(x => x.Path == "IsApprovedByRegulator"), isApprovedByRegulator != null);
        }
    }
}
