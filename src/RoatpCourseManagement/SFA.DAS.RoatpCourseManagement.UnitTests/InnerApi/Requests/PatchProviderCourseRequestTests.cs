using System.Linq;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
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
            Assert.That(request.Data.Count, Is.EqualTo(4));
            Assert.That(request.Data.Any(x => x.Path == "ContactUsEmail"), Is.True);
            Assert.That(request.Data.Any(x => x.Path == "ContactUsPhoneNumber"), Is.True);
            Assert.That(request.Data.Any(x => x.Path == "StandardInfoUrl"), Is.True);
            Assert.That(request.Data.Any(x => x.Path == "IsApprovedByRegulator"), Is.True);
            request.PatchUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}?userId={HttpUtility.UrlEncode(data.UserId)}&userDisplayName={HttpUtility.UrlEncode(data.UserDisplayName)}");
        }

        [TestCase("test@test.com", "1234567890", "http://www.google.com", true, 4)]
        [TestCase("test@test.com", "1234567890", "http://www.google.com", null, 3)]
        [TestCase(null, "1234567890", "http://www.google.com", true, 3)]
        [TestCase("test@test.com", null, "http://www.google.com", true, 3)]
        [TestCase("test@test.com", "1234567890", "http://www.google.com", true, 4)]
        [TestCase("test@test.com", "1234567890", null, true, 3)]
        [TestCase("test@test.com", "1234567890", null, null, 2)]
        [TestCase("test@test.com", "1234567890", "http://www.google.com", null, 3)]
        [TestCase("test@test.com", null, "http://www.google.com", null, 2)]
        [TestCase(null, "1234567890", "http://www.google.com", null, 2)]
        [TestCase(null, null, "http://www.google.com", true, 2)]
        [TestCase(null, "1234567890", "http://www.google.com", true, 3)]
        [TestCase("test@test.com", null, "http://www.google.com", true, 3)]
        [TestCase("test@test.com", "1234567890", null, true, 3)]
        [TestCase(null, null, "http://www.google.com", null, 1)]
        [TestCase(null, "1234567890", "http://www.google.com", null, 2)]
        [TestCase("test@test.com", null, "http://www.google.com", null, 2)]
        [TestCase("test@test.com", "1234567890", null, null, 2)]
        [TestCase(null, null, "http://www.google.com", true, 2)]
        [TestCase(null, "1234567890", null, true, 2)]
        [TestCase("test@test.com", null, null, true, 2)]
        [TestCase("test@test.com", null, null, null, 1)]
        [TestCase(null, "1234567890", null, null, 1)]
        [TestCase(null, null, null, null, 0)]
        [TestCase(null, null, null, true, 1)]
        public void Request_BuildDataPatchFromModel(string contactUsEmail, string contactUsPhoneNumber, string standardInfoUrl, bool? isApprovedByRegulator, int numberOfPatches)
        {
            var model = new ProviderCourseUpdateModel
            {
                ContactUsEmail = contactUsEmail,
                ContactUsPhoneNumber = contactUsPhoneNumber,
                StandardInfoUrl = standardInfoUrl,
                IsApprovedByRegulator = isApprovedByRegulator
            };
            var request = new PatchProviderCourseRequest(model);

            Assert.That(numberOfPatches, Is.EqualTo(request.Data.Count));
            Assert.That(request.Data.Any(x => x.Path == "ContactUsEmail"), Is.EqualTo(contactUsEmail != null));
            Assert.That(request.Data.Any(x => x.Path == "StandardInfoUrl"), Is.EqualTo(standardInfoUrl != null));
            Assert.That(request.Data.Any(x => x.Path == "ContactUsPhoneNumber"), Is.EqualTo(contactUsPhoneNumber != null));
            Assert.That(request.Data.Any(x => x.Path == "IsApprovedByRegulator"), Is.EqualTo(isApprovedByRegulator != null));
        }
    }
}
