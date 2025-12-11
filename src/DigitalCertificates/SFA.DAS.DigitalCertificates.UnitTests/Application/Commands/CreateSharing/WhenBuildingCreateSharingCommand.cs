using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharing;
using System;
using static SFA.DAS.DigitalCertificates.Models.Enums;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharing
{
    public class WhenBuildingCreateSharingCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(
            Guid userId,
            Guid certificateId,
            CertificateType certificateType,
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
    }
}