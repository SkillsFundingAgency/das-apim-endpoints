using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;
using System.Web;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class ProviderLocationBulkDeleteRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(ProviderLocationBulkDeleteModel data)
        {
            var request = new ProviderLocationBulkDeleteRequest(data);

            request.Ukprn.Should().Be(data.Ukprn);
            request.LarsCode.Should().Be(data.LarsCode);
            request.DeleteUrl.Should().Be($"/providers/{data.Ukprn}/locations/{data.LarsCode}/cleanup?userId={HttpUtility.UrlEncode(data.UserId)}");
        }
    }
}
