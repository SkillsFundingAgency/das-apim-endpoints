using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserAuthorise
{
    public class WhenBuildingCreateUserAuthoriseCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(Guid userId, long uln)
        {
            // Arrange & Act
            var command = new CreateUserAuthoriseCommand
            {
                UserId = userId,
                Uln = uln
            };

            // Assert
            command.UserId.Should().Be(userId);
            command.Uln.Should().Be(uln);
        }
    }
}
