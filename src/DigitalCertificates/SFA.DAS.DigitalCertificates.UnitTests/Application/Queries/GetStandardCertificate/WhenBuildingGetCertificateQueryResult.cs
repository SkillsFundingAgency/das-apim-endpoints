using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetStandardCertificate;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetStandardCertificate
{
    public class WhenBuildingGetCertificateQueryResult
    {
        [TestCase("Submitted", null, false)]
        [TestCase("PrintRequested", null, true)]
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

            var response = new GetStandardCertificateResponse
            {
                LatestEPAOutcome = "Pass",
                Status = status,
                PrintRequestedAt = hasPrintRequest ? requestedAt : null,
                PrintRequestedBy = hasPrintRequest ? "Apprentice" : null,
                PrintStatus = printStatus,
                PrintStatusAt = printStatus == null ? null : batchStatusAt
            };

            var result = (GetStandardCertificateQueryResult)response;

            result.LatestEPAOutcome.Should().Be("Pass");
            result.Status.Should().Be(status);
            result.PrintRequestedAt.Should().Be(response.PrintRequestedAt);
            result.PrintRequestedBy.Should().Be(response.PrintRequestedBy);
            result.PrintStatus.Should().Be(printStatus);
            result.PrintStatusAt.Should().Be(response.PrintStatusAt);
        }
    }
}