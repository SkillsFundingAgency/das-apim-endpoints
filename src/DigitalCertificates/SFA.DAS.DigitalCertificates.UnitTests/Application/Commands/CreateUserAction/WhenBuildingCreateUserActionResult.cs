using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserAction
{
    public class WhenBuildingCreateUserActionResult
    {
        [Test, AutoData]
        public void Then_Result_Properties_Are_Set_Correctly(string actionCode)
        {
            // Arrange & Act
            var result = new CreateUserActionResult
            {
                ActionCode = actionCode
            };

            // Assert
            result.ActionCode.Should().Be(actionCode);
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_CreateUserActionResponse_Correctly(
            CreateUserActionResponse response)
        {
            // Arrange & Act
            CreateUserActionResult result = response;

            // Assert
            result.ActionCode.Should().Be(response.ActionCode);
        }
    }
}
