using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetStandardCertificate;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetStandardCertificate
{
    public class WhenBuildingGetCertificateQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
            string familyName,
            string givenNames,
            long? uln,
            string certificateType,
            string courseCode,
            string courseName,
            string courseOption,
            int? courseLevel,
            DateTime dateAwarded,
            string overallGrade,
            string providerName,
            long ukprn,
            string employerName,
            string assessorName,
            DateTime startDate,
            DateTime? printRequestedAt,
            string printRequestedBy)
        {
            // Arrange & Act
            var result = new GetStandardCertificateQueryResult
            {
                FamilyName = familyName,
                GivenNames = givenNames,
                Uln = uln,
                CertificateType = certificateType,
                CourseCode = courseCode,
                CourseName = courseName,
                CourseOption = courseOption,
                CourseLevel = courseLevel,
                DateAwarded = dateAwarded,
                OverallGrade = overallGrade,
                ProviderName = providerName,
                Ukprn = ukprn,
                EmployerName = employerName,
                AssessorName = assessorName,
                StartDate = startDate,
                PrintRequestedAt = printRequestedAt,
                PrintRequestedBy = printRequestedBy,
                DeliveryInformation = null
            };

            // Assert
            result.FamilyName.Should().Be(familyName);
            result.GivenNames.Should().Be(givenNames);
            result.Uln.Should().Be(uln);
            result.CertificateType.Should().Be(certificateType);
            result.CourseCode.Should().Be(courseCode);
            result.CourseName.Should().Be(courseName);
            result.CourseOption.Should().Be(courseOption);
            result.CourseLevel.Should().Be(courseLevel);
            result.DateAwarded.Should().Be(dateAwarded);
            result.OverallGrade.Should().Be(overallGrade);
            result.ProviderName.Should().Be(providerName);
            result.Ukprn.Should().Be(ukprn);
            result.EmployerName.Should().Be(employerName);
            result.AssessorName.Should().Be(assessorName);
            result.StartDate.Should().Be(startDate);
            result.PrintRequestedAt.Should().Be(printRequestedAt);
            result.PrintRequestedBy.Should().Be(printRequestedBy);
            result.DeliveryInformation.Should().BeNull();
        }
    }
}
