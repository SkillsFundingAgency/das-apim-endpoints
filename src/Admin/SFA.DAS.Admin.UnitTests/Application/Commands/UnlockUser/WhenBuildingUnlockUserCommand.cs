using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.Application.Commands.UnlockUser;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Admin.UnitTests.Application.Commands.UnlockUser
{
    public class WhenBuildingUnlockUserCommand
    {
        [Test, MoqAutoData]
        public void Then_Properties_Can_Be_Set_And_Read(UnlockUserCommand command)
        {
            // Arrange
            var userId = Guid.NewGuid();
            var username = "test.user";
            var userActionId = 123L;

            // Act
            command.UserId = userId;
            command.Username = username;
            command.UserActionId = userActionId;

            // Assert
            command.UserId.Should().Be(userId);
            command.Username.Should().Be(username);
            command.UserActionId.Should().Be(userActionId);
        }
    }
}
