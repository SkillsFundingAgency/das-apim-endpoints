using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetFrameworkCertificateMasksRequest
    {
        [Test]
        public void Then_The_GetUrl_Contains_Single_Uln()
        {
            var request = new GetFrameworkCertificateMasksRequest(new List<long> { 1000000001 });

            request.GetUrl.Should().Be("api/v1/certificates/framework/search/masks?exclude=1000000001");
        }

        [Test]
        public void Then_The_GetUrl_Contains_Multiple_Ulns()
        {
            var request = new GetFrameworkCertificateMasksRequest(new List<long> { 1000000001, 1000000002 });

            request.GetUrl.Should().Be("api/v1/certificates/framework/search/masks?exclude=1000000001&exclude=1000000002");
        }
    }
}
