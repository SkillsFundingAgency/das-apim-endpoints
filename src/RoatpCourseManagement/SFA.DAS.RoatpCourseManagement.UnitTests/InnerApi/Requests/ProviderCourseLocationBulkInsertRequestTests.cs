using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class ProviderCourseLocationBulkInsertRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(ProviderCourseLocationBulkInsertModel data)
        {
            var request = new ProviderCourseLocationBulkInsertRequest(data);

            request.Data.Should().Be(data);
            request.Ukprn.Should().Be(data.Ukprn);
            request.LarsCode.Should().Be(data.LarsCode);
            request.PostUrl.Should().Be($"providers/{data.Ukprn}/courses/{data.LarsCode}/locations/bulk-insert-regions");
        }
    }
}
