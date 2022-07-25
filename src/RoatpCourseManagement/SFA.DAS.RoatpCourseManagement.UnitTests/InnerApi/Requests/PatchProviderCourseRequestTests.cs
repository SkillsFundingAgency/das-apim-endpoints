using System.Linq;
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
            Assert.AreEqual(5, request.Data.Count);
            Assert.IsTrue(request.Data.Any(x => x.Path == "ContactUsEmail"));
            Assert.IsTrue(request.Data.Any(x => x.Path == "ContactUsPageUrl"));
            Assert.IsTrue(request.Data.Any(x => x.Path == "ContactUsPhoneNumber"));
            Assert.IsTrue(request.Data.Any(x => x.Path == "StandardInfoUrl"));
            Assert.IsTrue(request.Data.Any(x => x.Path == "IsApprovedByRegulator"));
            request.PatchUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}");
        }
    }
}
