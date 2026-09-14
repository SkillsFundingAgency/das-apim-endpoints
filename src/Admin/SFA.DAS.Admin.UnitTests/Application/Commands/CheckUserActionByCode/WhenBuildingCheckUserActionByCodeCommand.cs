using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.Application.Commands.CheckUserActionByCode;

namespace SFA.DAS.Admin.UnitTests.Application.Commands.CheckUserActionByCode
{
    public class WhenBuildingCheckUserActionByCodeCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(
            string code,
            string username)
        {
            // Arrange & Act
            var command = new CheckUserActionByCodeCommand
            {
                Code = code,
                Username = username
            };

            // Assert
            command.Code.Should().Be(code);
            command.Username.Should().Be(username);
        }
    }
}
