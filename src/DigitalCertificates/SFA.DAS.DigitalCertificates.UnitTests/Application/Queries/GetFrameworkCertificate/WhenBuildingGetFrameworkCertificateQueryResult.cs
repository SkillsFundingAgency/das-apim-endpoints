using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetFrameworkCertificate
{
    public class WhenBuildingGetFrameworkCertificateQueryResult
    {
        [TestCase(null, null, false)]
        [TestCase("Reprint", null, true)]
        [TestCase("SentToPrinter", "SentToPrinter", true)]
        [TestCase("Printed", "Printed", true)]
        [TestCase("NotDelivered", "NotDelivered", true)]
        [TestCase("Delivered", "Delivered", true)]
        [TestCase("Delivered", null, true)]
        public void Then_Request_And_Batch_Details_Are_Mapped_Independently(
            string status,
            string printStatus,
            bool hasPrintRequest)
        {
            var requestedAt = new DateTime(2026, 10, 1, 9, 0, 0, DateTimeKind.Utc);
            var batchStatusAt = new DateTime(2026, 10, 2, 10, 0, 0, DateTimeKind.Utc);

            var response = new GetFrameworkCertificateResponse
            {
                CertificateStatus = status,
                CertificatePrintRequestedAt = hasPrintRequest ? requestedAt : null,
                CertificatePrintRequestedBy = hasPrintRequest ? "Admin user" : null,
                CertificatePrintStatus = printStatus,
                CertificatePrintStatusAt = printStatus == null ? null : batchStatusAt
            };

            var result = (GetFrameworkCertificateQueryResult)response;

            result.Status.Should().Be(status);
            result.PrintRequestedAt.Should().Be(response.CertificatePrintRequestedAt);
            result.PrintRequestedBy.Should().Be(response.CertificatePrintRequestedBy);
            result.PrintStatus.Should().Be(printStatus);
            result.PrintStatusAt.Should().Be(response.CertificatePrintStatusAt);
        }
    }
}