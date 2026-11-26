using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.DeleteSharing;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.DeleteSharing
{
    public class WhenBuildingDeleteSharingCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(Guid sharingId)
        {
            var command = new DeleteSharingCommand { SharingId = sharingId };

            command.SharingId.Should().Be(sharingId);
        }
    }
}
