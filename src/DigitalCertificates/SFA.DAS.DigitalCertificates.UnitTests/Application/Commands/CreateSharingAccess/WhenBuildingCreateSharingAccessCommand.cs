using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateSharingAccess;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateSharingAccess
{
    public class WhenBuildingCreateSharingAccessCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(Guid sharingId)
        {
            // Arrange & Act
            var command = new CreateSharingAccessCommand
            {
                SharingId = sharingId
            };

            // Assert
            command.SharingId.Should().Be(sharingId);
        }
    }
}
