using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAction;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostCreateUserActionRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(Guid userId)
        {
            // Arrange & Act
            var request = new PostCreateUserActionRequest(new PostCreateUserActionRequestData(), userId);

            // Assert
            request.PostUrl.Should().Be($"api/users/{userId}/actions");
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_CreateUserActionCommand_Correctly(CreateUserActionCommand command)
        {
            // Arrange & Act
            PostCreateUserActionRequestData requestData = command;

            // Assert
            requestData.ActionType.Should().Be(command.ActionType);
            requestData.FamilyName.Should().Be(command.FamilyName);
            requestData.GivenNames.Should().Be(command.GivenNames);
            requestData.CertificateId.Should().Be(command.CertificateId);
            requestData.CertificateType.Should().Be(command.CertificateType);
            requestData.CourseName.Should().Be(command.CourseName);
        }
    }
}
