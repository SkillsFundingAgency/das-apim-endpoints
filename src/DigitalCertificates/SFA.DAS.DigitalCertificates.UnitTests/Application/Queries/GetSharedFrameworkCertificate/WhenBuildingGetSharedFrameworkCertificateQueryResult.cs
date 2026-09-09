using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedFrameworkCertificate;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkCertificate;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharedFrameworkCertificate
{
    public class WhenBuildingGetSharedFrameworkCertificateQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
            string familyName,
            string givenNames,
            string certificateType,
            System.Collections.Generic.List<QualificationDetails> qualificationsAndAwardingBodies,
            string certificateReference,
            string frameworkCertificateNumber,
            string courseName,
            string courseOption,
            string courseLevel,
            DateTime dateAwarded,
            string providerName,
            string employerName,
            DateTime startDate)
        {
            // Arrange & Act
            var result = new GetSharedFrameworkCertificateQueryResult
            {
                FamilyName = familyName,
                GivenNames = givenNames,
                CertificateType = certificateType,
                QualificationsAndAwardingBodies = qualificationsAndAwardingBodies,
                CertificateReference = certificateReference,
                FrameworkCertificateNumber = frameworkCertificateNumber,
                CourseName = courseName,
                CourseOption = courseOption,
                CourseLevel = courseLevel,
                DateAwarded = dateAwarded,
                ProviderName = providerName,
                EmployerName = employerName,
                StartDate = startDate
            };

            // Assert
            result.FamilyName.Should().Be(familyName);
            result.GivenNames.Should().Be(givenNames);
            result.CertificateType.Should().Be(certificateType);
            result.CertificateReference.Should().Be(certificateReference);
            result.FrameworkCertificateNumber.Should().Be(frameworkCertificateNumber);
            result.CourseName.Should().Be(courseName);
            result.CourseOption.Should().Be(courseOption);
            result.CourseLevel.Should().Be(courseLevel);
            result.DateAwarded.Should().Be(dateAwarded);
            result.ProviderName.Should().Be(providerName);
            result.EmployerName.Should().Be(employerName);
            result.StartDate.Should().Be(startDate);
        }
    }
}
