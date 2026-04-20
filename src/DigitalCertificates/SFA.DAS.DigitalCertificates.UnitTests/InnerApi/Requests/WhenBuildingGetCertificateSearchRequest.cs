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
        public void Then_The_GetUrl_Contains_Dob_And_Name()
        {
            var dob = new DateTime(1990, 6, 15);
            var request = new GetCertificateSearchRequest(dob, "Smith");

            request.GetUrl.Should().Be("api/v1/certificates/search?dob=1990-06-15&name=Smith");
        }

        [Test]
        public void Then_The_GetUrl_Includes_Exclude_Ulns()
        {
            var dob = new DateTime(1985, 1, 30);
            var request = new GetCertificateSearchRequest(dob, "Jones", new List<long> { 1000000001, 1000000002 });

            request.GetUrl.Should().Be("api/v1/certificates/search?dob=1985-01-30&name=Jones&exclude=1000000001&exclude=1000000002");
        }

        [Test]
        public void Then_The_GetUrl_Encodes_Special_Characters_In_Name()
        {
            var dob = new DateTime(2000, 3, 1);
            var request = new GetCertificateSearchRequest(dob, "O'Brien");

            request.GetUrl.Should().Be("api/v1/certificates/search?dob=2000-03-01&name=O%27Brien");
        }
    }
}
