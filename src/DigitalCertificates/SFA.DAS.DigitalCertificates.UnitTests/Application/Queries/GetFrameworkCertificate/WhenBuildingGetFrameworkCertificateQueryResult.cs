using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetFrameworkCertificate
{
    public class WhenBuildingGetFrameworkCertificateQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
            string familyName,
            string givenNames,
            long? uln,
            string certificateReference,
            string frameworkCertificateNumber,
            string courseName,
            string pathwayName,
            string courseLevel,
            DateTime dateAwarded,
            string providerName,
            string employerName,
            DateTime startDate,
            DateTime? printRequestedAt,
            string printRequestedBy)
        {
            // Arrange & Act
            var result = new GetFrameworkCertificateQueryResult
            {
                FamilyName = familyName,
                GivenNames = givenNames,
                Uln = uln,
                CertificateReference = certificateReference,
                FrameworkCertificateNumber = frameworkCertificateNumber,
                CourseName = courseName,
                CourseOption = pathwayName,
                CourseLevel = courseLevel,
                DateAwarded = dateAwarded,
                ProviderName = providerName,
                EmployerName = employerName,
                StartDate = startDate,
                PrintRequestedAt = printRequestedAt,
                PrintRequestedBy = printRequestedBy,
                DeliveryInformation = null
            };

            // Assert
            result.FamilyName.Should().Be(familyName);
            result.GivenNames.Should().Be(givenNames);
            result.Uln.Should().Be(uln);
            result.CertificateReference.Should().Be(certificateReference);
            result.FrameworkCertificateNumber.Should().Be(frameworkCertificateNumber);
            result.CourseName.Should().Be(courseName);
            result.CourseOption.Should().Be(pathwayName);
            result.CourseLevel.Should().Be(courseLevel);
            result.DateAwarded.Should().Be(dateAwarded);
            result.ProviderName.Should().Be(providerName);
            result.EmployerName.Should().Be(employerName);
            result.StartDate.Should().Be(startDate);
            result.PrintRequestedAt.Should().Be(printRequestedAt);
            result.PrintRequestedBy.Should().Be(printRequestedBy);
            result.DeliveryInformation.Should().BeNull();
        }

        [Test]
        public void Then_Irrelevant_Log_Entries_Are_Excluded()
        {
            var response = new GetFrameworkCertificateResponse
            {
                CertificateLogs = new List<CertificateLog>
                {
                    new CertificateLog { Id = Guid.NewGuid(), Action = "Start", Status = "Reprint", EventTime = DateTime.UtcNow }
                }
            };

            var result = (GetFrameworkCertificateQueryResult)response;

            result.DeliveryInformation.Should().BeEmpty();
        }

        [Test]
        public void Then_All_Relevant_Action_Status_Pairs_Are_Included()
        {
            var submitId = Guid.NewGuid();
            var sentToPrinterId = Guid.NewGuid();
            var printedId = Guid.NewGuid();
            var deliveredId = Guid.NewGuid();
            var reprintId = Guid.NewGuid();
            var baseTime = new DateTime(2026, 1, 1, 0, 0, 0);

            var response = new GetFrameworkCertificateResponse
            {
                CertificateLogs = new List<CertificateLog>
                {
                    new CertificateLog { Id = submitId,        Action = "Submit",  Status = "Submitted",     EventTime = baseTime },
                    new CertificateLog { Id = sentToPrinterId, Action = "Status",  Status = "SentToPrinter", EventTime = baseTime.AddDays(1) },
                    new CertificateLog { Id = printedId,       Action = "Printed", Status = "Printed",       EventTime = baseTime.AddDays(2) },
                    new CertificateLog { Id = deliveredId,     Action = "Status",  Status = "Delivered",     EventTime = baseTime.AddDays(3) },
                    new CertificateLog { Id = reprintId,       Action = "Reprint", Status = "Reprint",       EventTime = baseTime.AddDays(4) },
                }
            };

            var result = (GetFrameworkCertificateQueryResult)response;

            result.DeliveryInformation.Should().HaveCount(5);
            result.DeliveryInformation.Should().Contain(d => d.Id == submitId        && d.Action == "Submit"  && d.Status == "Submitted");
            result.DeliveryInformation.Should().Contain(d => d.Id == sentToPrinterId && d.Action == "Status"  && d.Status == "SentToPrinter");
            result.DeliveryInformation.Should().Contain(d => d.Id == printedId       && d.Action == "Printed" && d.Status == "Printed");
            result.DeliveryInformation.Should().Contain(d => d.Id == deliveredId     && d.Action == "Status"  && d.Status == "Delivered");
            result.DeliveryInformation.Should().Contain(d => d.Id == reprintId       && d.Action == "Reprint" && d.Status == "Reprint");
        }

        [Test]
        public void Then_Duplicate_Action_Status_Entries_Keep_Only_Latest_By_EventTime()
        {
            var olderReprintId = Guid.NewGuid();
            var latestReprintId = Guid.NewGuid();
            var baseTime = new DateTime(2026, 1, 1, 0, 0, 0);

            var response = new GetFrameworkCertificateResponse
            {
                CertificateLogs = new List<CertificateLog>
                {
                    new CertificateLog { Id = olderReprintId,  Action = "Reprint", Status = "Reprint", EventTime = baseTime },
                    new CertificateLog { Id = latestReprintId, Action = "Reprint", Status = "Reprint", EventTime = baseTime.AddDays(5) },
                }
            };

            var result = (GetFrameworkCertificateQueryResult)response;

            result.DeliveryInformation.Should().HaveCount(1);
            result.DeliveryInformation[0].Id.Should().Be(latestReprintId);
        }

        [Test]
        public void Then_DeliveryInformation_Is_Ordered_Chronologically_By_EventTime()
        {
            var baseTime = new DateTime(2026, 1, 1, 0, 0, 0);
            var reprintId = Guid.NewGuid();
            var sentToPrinterId = Guid.NewGuid();
            var printedId = Guid.NewGuid();
            var deliveredId = Guid.NewGuid();

            var response = new GetFrameworkCertificateResponse
            {
                CertificateLogs = new List<CertificateLog>
                {
                    new CertificateLog { Id = deliveredId,     Action = "Status",  Status = "Delivered",     EventTime = baseTime.AddDays(3) },
                    new CertificateLog { Id = reprintId,       Action = "Reprint", Status = "Reprint",       EventTime = baseTime.AddDays(4) },
                    new CertificateLog { Id = sentToPrinterId, Action = "Status",  Status = "SentToPrinter", EventTime = baseTime },
                    new CertificateLog { Id = printedId,       Action = "Printed", Status = "Printed",       EventTime = baseTime.AddDays(2) },
                }
            };

            var result = (GetFrameworkCertificateQueryResult)response;

            result.DeliveryInformation.Should().HaveCount(4);
            result.DeliveryInformation[0].Id.Should().Be(reprintId);
            result.DeliveryInformation[1].Id.Should().Be(deliveredId);
            result.DeliveryInformation[2].Id.Should().Be(printedId);
            result.DeliveryInformation[3].Id.Should().Be(sentToPrinterId);
        }
    }
}
