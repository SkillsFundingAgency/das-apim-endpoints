using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetCertificateSearchRequest
    {
        [Test]
        public void Then_The_GetUrl_Contains_Dob_And_Encoded_Name_When_No_Excludes()
        {
            var date = new DateTime(1990, 1, 2);
            var request = new GetCertificateSearchRequest(date, "O'Neil", null);

            request.GetUrl.Should().Be("api/v1/certificates/search?dob=1990-01-02&name=O%27Neil");
        }

        [Test]
        public void Then_The_GetUrl_Contains_Single_Exclude()
        {
            var date = new DateTime(1990, 1, 2);
            var request = new GetCertificateSearchRequest(date, "Smith", new List<long> { 1000000001 });

            request.GetUrl.Should().Be("api/v1/certificates/search?dob=1990-01-02&name=Smith&exclude=1000000001");
        }

        [Test]
        public void Then_The_GetUrl_Contains_Multiple_Excludes()
        {
            var date = new DateTime(1990, 1, 2);
            var request = new GetCertificateSearchRequest(date, "Smith", new List<long> { 1000000001, 1000000002 });

            request.GetUrl.Should().Be("api/v1/certificates/search?dob=1990-01-02&name=Smith&exclude=1000000001&exclude=1000000002");
        }
    }
}

