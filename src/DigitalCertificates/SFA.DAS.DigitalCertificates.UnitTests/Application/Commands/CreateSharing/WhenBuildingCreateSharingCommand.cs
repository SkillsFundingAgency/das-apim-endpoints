using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharing
{
    public class WhenBuildingCreateSharingCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(
            Guid userId,
            Guid certificateId,
            string certificateType,
            string courseName)
        {
            // Arrange & Act
            var command = new CreateSharingCommand
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName
            };

            // Assert
            command.UserId.Should().Be(userId);
            command.CertificateId.Should().Be(certificateId);
            command.CertificateType.Should().Be(certificateType);
            command.CourseName.Should().Be(courseName);
        }

        [TestCase("Software tester <script>alert(1)</script>")]
        [TestCase("<img src=x onerror=alert(1)>")]
        [TestCase("Name <b>bold</b>")]
        public void Then_CourseName_With_Html_Tags_Fails_Validation(string maliciousCourseName)
        {
            var command = new CreateSharingCommand { CourseName = maliciousCourseName };
            var results = new List<ValidationResult>();
            var context = new ValidationContext(command) { MemberName = nameof(command.CourseName) };

            var isValid = Validator.TryValidateProperty(command.CourseName, context, results);

            isValid.Should().BeFalse();
            results.Should().ContainSingle(r => r.ErrorMessage == "CourseName contains invalid characters.");
        }

        [TestCase("Software Tester")]
        [TestCase("Science & Technology")]
        [TestCase("C# Programming (Level 3)")]
        public void Then_CourseName_Without_Html_Tags_Passes_Validation(string validCourseName)
        {
            var command = new CreateSharingCommand { CourseName = validCourseName };
            var results = new List<ValidationResult>();
            var context = new ValidationContext(command) { MemberName = nameof(command.CourseName) };

            var isValid = Validator.TryValidateProperty(command.CourseName, context, results);

            isValid.Should().BeTrue();
            results.Should().BeEmpty();
        }
    }
}