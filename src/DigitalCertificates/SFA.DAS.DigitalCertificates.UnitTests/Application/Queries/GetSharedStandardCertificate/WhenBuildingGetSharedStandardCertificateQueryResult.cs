using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharedStandardCertificate;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharedStandardCertificate
{
    public class WhenBuildingGetSharedStandardCertificateQueryResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(
            string familyName,
            string givenNames,
            string certificateType,
            string courseName,
            string courseOption,
            int? courseLevel,
            DateTime dateAwarded,
            string overallGrade,
            string providerName,
            DateTime startDate)
        {
            // Arrange & Act
            var result = new GetSharedStandardCertificateQueryResult
            {
                FamilyName = familyName,
                GivenNames = givenNames,
                CertificateType = certificateType,
                CourseName = courseName,
                CourseOption = courseOption,
                CourseLevel = courseLevel,
                DateAwarded = dateAwarded,
                OverallGrade = overallGrade,
                ProviderName = providerName,
                StartDate = startDate,

            };

            // Assert
            result.FamilyName.Should().Be(familyName);
            result.GivenNames.Should().Be(givenNames);
            result.CertificateType.Should().Be(certificateType);
            result.CourseName.Should().Be(courseName);
            result.CourseOption.Should().Be(courseOption);
            result.CourseLevel.Should().Be(courseLevel);
            result.DateAwarded.Should().Be(dateAwarded);
            result.OverallGrade.Should().Be(overallGrade);
            result.ProviderName.Should().Be(providerName);
            result.StartDate.Should().Be(startDate);
        }
    }
}
