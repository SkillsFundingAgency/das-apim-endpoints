using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;

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
    }
}
