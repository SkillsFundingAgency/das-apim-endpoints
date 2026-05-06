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
            int? dateAwarded,
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
                FamilyName = familyName,
                DateOfBirth = dateOfBirth,
                CertificateType = certificateType,
                CourseCode = courseCode,
                CourseName = courseName,
                CourseLevel = courseLevel,
                DateAwarded = dateAwarded,
                ProviderName = providerName,
                Ukprn = ukprn,
                IsMatched = isMatched,
                IsFailed = isFailed
            };

            // Assert
            command.UserId.Should().Be(userId);
            command.Uln.Should().Be(uln);
            command.FamilyName.Should().Be(familyName);
            command.DateOfBirth.Should().Be(dateOfBirth);
            command.CertificateType.Should().Be(certificateType);
            command.CourseCode.Should().Be(courseCode);
            command.CourseName.Should().Be(courseName);
            command.CourseLevel.Should().Be(courseLevel);
            command.DateAwarded.Should().Be(dateAwarded);
            command.ProviderName.Should().Be(providerName);
            command.Ukprn.Should().Be(ukprn);
            command.IsMatched.Should().Be(isMatched);
            command.IsFailed.Should().Be(isFailed);
        }
    }
}
