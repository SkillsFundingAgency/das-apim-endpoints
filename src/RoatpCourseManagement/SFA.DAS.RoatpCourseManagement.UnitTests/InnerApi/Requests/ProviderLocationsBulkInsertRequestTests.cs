using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Requests;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class ProviderLocationsBulkInsertRequestTests
    {
        [Test, AutoData]
        public void Constructor_ConstructsRequest(ProviderLocationBulkInsertModel data)
        {
            var request = new ProviderLocationsBulkInsertRequest(data);

            request.Data.Should().Be(data);
            request.Ukprn.Should().Be(data.Ukprn);
            request.LarsCode.Should().Be(data.LarsCode);
            request.PostUrl.Should().Be($"providers/{data.Ukprn}/locations/bulk-insert-regions");
        }
    }
}
