using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificateSharing;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostCreateCertificateSharingRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built()
        {
            var request = new PostCreateSharingRequest(new PostCreateSharingRequestData());

            request.PostUrl.Should().Be("api/sharing");
        }

        [Test, AutoData]
        public void Then_Implicit_Operator_Maps_CreateCertificateSharingCommand_Correctly(
            CreateCertificateSharingCommand command)
        {
            PostCreateSharingRequestData requestData = command;

            requestData.UserId.Should().Be(command.UserId);
            requestData.CertificateId.Should().Be(command.CertificateId);
            requestData.CertificateType.Should().Be(command.CertificateType);
            requestData.CourseName.Should().Be(command.CourseName);
        }
    }
}