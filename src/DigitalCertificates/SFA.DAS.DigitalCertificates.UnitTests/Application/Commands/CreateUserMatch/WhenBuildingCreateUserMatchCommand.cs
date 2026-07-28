using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserMatch
{
    public class WhenBuildingCreateUserMatchCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(
            Guid userId,
            long uln,
            string familyName,
            DateTime dateOfBirth,
            string certificateType,
            string courseCode,
            string courseName,
            string courseLevel,
            int? yearAwarded,
            string providerName,
            int? ukprn,
            bool isMatched,
            bool isFailed)
        {
            // Arrange & Act
            var command = new CreateUserMatchCommand
            {
                UserId = userId,
                Uln = uln,
                CertificateType = certificateType,
                CourseCode = courseCode,
                CourseName = courseName,
                CourseLevel = courseLevel,
                YearAwarded = yearAwarded,
                ProviderName = providerName,
                Ukprn = ukprn,
                IsMatched = isMatched,
                IsFailed = isFailed
            };

            // Assert
            command.UserId.Should().Be(userId);
            command.Uln.Should().Be(uln);
            command.CertificateType.Should().Be(certificateType);
            command.CourseCode.Should().Be(courseCode);
            command.CourseName.Should().Be(courseName);
            command.CourseLevel.Should().Be(courseLevel);
            command.YearAwarded.Should().Be(yearAwarded);
            command.ProviderName.Should().Be(providerName);
            command.Ukprn.Should().Be(ukprn);
            command.IsMatched.Should().Be(isMatched);
            command.IsFailed.Should().Be(isFailed);
        }
    }
}
