using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserAction
{
    public class WhenBuildingCreateUserActionCommand
    {
        [Test, AutoData]
        public void Then_Command_Properties_Are_Set_Correctly(
            Guid userId,
            string actionType,
            string familyName,
            string givenNames,
            Guid? certificateId,
            string certificateType,
            string courseName)
        {
            // Arrange & Act
            var command = new CreateUserActionCommand
            {
                UserId = userId,
                ActionType = actionType,
                FamilyName = familyName,
                GivenNames = givenNames,
                CertificateId = certificateId,
                CertificateType = certificateType,
                CourseName = courseName
            };

            // Assert
            command.UserId.Should().Be(userId);
            command.ActionType.Should().Be(actionType);
            command.FamilyName.Should().Be(familyName);
            command.GivenNames.Should().Be(givenNames);
            command.CertificateId.Should().Be(certificateId);
            command.CertificateType.Should().Be(certificateType);
            command.CourseName.Should().Be(courseName);
        }
    }
}
