using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostAuthoriseUserRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(Guid userId)
        {
            var request = new PostAuthoriseUserRequest(new PostAuthoriseUserRequestData(), userId);

            request.PostUrl.Should().Be($"api/users/{userId}/authorise");
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_CreateUserAuthoriseCommand_Correctly(CreateUserAuthoriseCommand command)
        {
            PostAuthoriseUserRequestData requestData = command;

            requestData.Uln.Should().Be(command.Uln);
        }
    }
}
