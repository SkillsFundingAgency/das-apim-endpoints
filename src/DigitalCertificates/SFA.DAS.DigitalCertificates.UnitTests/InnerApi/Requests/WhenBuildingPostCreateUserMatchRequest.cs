using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserMatch;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostCreateUserMatchRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(Guid userId)
        {
            var request = new PostCreateUserMatchRequest(new PostCreateUserMatchRequestData(), userId);

            request.PostUrl.Should().Be($"api/users/{userId}/match");
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_CreateUserMatchCommand_Correctly(CreateUserMatchCommand command)
        {
            PostCreateUserMatchRequestData requestData = command;

            requestData.Uln.Should().Be(command.Uln);
            requestData.FamilyName.Should().Be(command.FamilyName);
            requestData.DateOfBirth.Should().Be(command.DateOfBirth);
            requestData.CertificateType.Should().Be(command.CertificateType);
            requestData.CourseCode.Should().Be(command.CourseCode);
            requestData.CourseName.Should().Be(command.CourseName);
            requestData.CourseLevel.Should().Be(command.CourseLevel);
            requestData.DateAwarded.Should().Be(command.DateAwarded);
            requestData.ProviderName.Should().Be(command.ProviderName);
            requestData.Ukprn.Should().Be(command.Ukprn);
            requestData.IsMatched.Should().Be(command.IsMatched);
            requestData.IsFailed.Should().Be(command.IsFailed);
        }
    }
}
