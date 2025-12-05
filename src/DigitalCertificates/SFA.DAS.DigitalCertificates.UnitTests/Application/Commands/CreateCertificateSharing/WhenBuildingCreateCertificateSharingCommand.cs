using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;
using System;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateCertificateSharing
{
    public class WhenBuildingCreateCertificateSharingCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(
            Guid userId,
            Guid certificateId,
            string certificateType,
            string courseName)
        {
            var command = new CreateCertificateSharingCommand
            {
                UserId = userId,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName
            };

            command.UserId.Should().Be(userId);
            command.CertificateId.Should().Be(certificateId);
            command.CertificateType.Should().Be(certificateType);
            command.CourseName.Should().Be(courseName);
        }
    }
}