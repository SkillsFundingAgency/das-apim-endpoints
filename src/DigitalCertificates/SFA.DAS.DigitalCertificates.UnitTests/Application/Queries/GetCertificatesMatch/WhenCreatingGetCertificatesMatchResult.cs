using System;
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetCertificatesMatch;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetCertificatesMatch
{
    public class WhenCreatingGetCertificatesMatchResult
    {
        [Test]
        public void Then_Result_Properties_Are_Set_Correctly()
        {
            // Arrange
            var match = new CertificateMatchResult
            {
                Uln = 1234567890,
                CertificateType = "Standard",
                CourseCode = "ABC",
                CourseName = "Course A",
                CourseLevel = "3",
                DateAwarded = new DateTime(2020,1,1),
                ProviderName = "Provider X"
            };

            var mask = new CertificateMaskResult
            {
                CertificateType = "Standard",
                CourseCode = "ABC",
                CourseName = "Course A",
                CourseLevel = "3",
                ProviderName = "Provider X"
            };

            // Act
            var result = new GetCertificatesMatchResult
            {
                Matches = new List<CertificateMatchResult> { match },
                Masks = new List<CertificateMaskResult> { mask }
            };

            // Assert
            result.Matches.Should().HaveCount(1);
            result.Matches[0].Should().BeEquivalentTo(match);

            result.Masks.Should().HaveCount(1);
            result.Masks[0].Should().BeEquivalentTo(mask);
        }
    }
}
