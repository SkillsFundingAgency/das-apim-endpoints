using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingEmailAccess;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharingEmailAccess
{
    public class WhenBuildingCreateSharingEmailAccessCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(Guid sharingEmailId)
        {
            // Arrange & Act
            var command = new CreateSharingEmailAccessCommand
            {
                SharingEmailId = sharingEmailId
            };

            // Assert
            command.SharingEmailId.Should().Be(sharingEmailId);
        }
    }
}
