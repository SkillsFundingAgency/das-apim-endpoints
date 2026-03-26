using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateCertificatePrintRequest;
using SFA.DAS.DigitalCertificates.InnerApi.Requests.Assessor;
using SFA.DAS.DigitalCertificates.InnerApi.Responses.Assessor;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateCertificatePrintRequest
{
    public class WhenHandlingCreateCertificatePrintRequestCommand
    {
        [Test, MoqAutoData]
        public async Task Then_PutWithResponseCode_Is_Called_When_Certificate_Is_Valid(
            CreateCertificatePrintRequestCommand command,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            CreateCertificatePrintRequestCommandHandler handler)
        {
            // Arrange
            var certificate = new GetStandardCertificateResponse
            {
                LatestEPAOutcome = "Pass",
                Status = "Submitted",
                PrintRequestedAt = null
            };

            var getResponse = new ApiResponse<GetStandardCertificateResponse>(certificate, HttpStatusCode.OK, string.Empty);
            var putResponse = new ApiResponse<NullResponse>(null, HttpStatusCode.NoContent, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetStandardCertificateResponse>(
                    It.Is<GetStandardCertificateRequest>(r => r.Id == command.CertificateId)))
                .ReturnsAsync(getResponse);

            mockAssessorsApiClient
                .Setup(c => c.PutWithResponseCode<PutCertificatePrintRequestData, NullResponse>(
                    It.Is<PutCertificatePrintRequest>(r => r.PutUrl.Contains(command.CertificateId.ToString()))))
                .ReturnsAsync(putResponse);

            // Act / Assert
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);
            await act.Should().NotThrowAsync<Exception>();

            mockAssessorsApiClient.Verify(c =>
                c.PutWithResponseCode<PutCertificatePrintRequestData, NullResponse>(
                    It.Is<PutCertificatePrintRequest>(r => r.PutUrl.Contains(command.CertificateId.ToString()))),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_ArgumentException_Thrown_When_Certificate_Not_Found(
            CreateCertificatePrintRequestCommand command,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            CreateCertificatePrintRequestCommandHandler handler)
        {
            // Arrange
            var getResponse = new ApiResponse<GetStandardCertificateResponse>(null, HttpStatusCode.NotFound, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetStandardCertificateResponse>(
                    It.IsAny<GetStandardCertificateRequest>()))
                .ReturnsAsync(getResponse);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Certificate not found");
        }

        [Test, MoqAutoData]
        public async Task Then_ArgumentException_Thrown_When_EPA_Outcome_Is_Not_Pass(
            CreateCertificatePrintRequestCommand command,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            CreateCertificatePrintRequestCommandHandler handler)
        {
            // Arrange
            var certificate = new GetStandardCertificateResponse
            {
                LatestEPAOutcome = "Fail",
                Status = "Submitted",
                PrintRequestedAt = null
            };

            var getResponse = new ApiResponse<GetStandardCertificateResponse>(certificate, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetStandardCertificateResponse>(
                    It.IsAny<GetStandardCertificateRequest>()))
                .ReturnsAsync(getResponse);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Certificate is not eligible for print request");
        }

        [Test, MoqAutoData]
        public async Task Then_ArgumentException_Thrown_When_Status_Is_Not_Submitted(
            CreateCertificatePrintRequestCommand command,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            CreateCertificatePrintRequestCommandHandler handler)
        {
            // Arrange
            var certificate = new GetStandardCertificateResponse
            {
                LatestEPAOutcome = "Pass",
                Status = "Draft",
                PrintRequestedAt = null
            };

            var getResponse = new ApiResponse<GetStandardCertificateResponse>(certificate, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetStandardCertificateResponse>(
                    It.IsAny<GetStandardCertificateRequest>()))
                .ReturnsAsync(getResponse);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Certificate is not eligible for print request");
        }

        [Test, MoqAutoData]
        public async Task Then_ArgumentException_Thrown_When_Print_Already_Requested(
            CreateCertificatePrintRequestCommand command,
            [Frozen] Mock<IAssessorsApiClient<AssessorsApiConfiguration>> mockAssessorsApiClient,
            CreateCertificatePrintRequestCommandHandler handler)
        {
            // Arrange
            var certificate = new GetStandardCertificateResponse
            {
                LatestEPAOutcome = "Pass",
                Status = "Submitted",
                PrintRequestedAt = DateTime.UtcNow
            };

            var getResponse = new ApiResponse<GetStandardCertificateResponse>(certificate, HttpStatusCode.OK, string.Empty);

            mockAssessorsApiClient
                .Setup(c => c.GetWithResponseCode<GetStandardCertificateResponse>(
                    It.IsAny<GetStandardCertificateRequest>()))
                .ReturnsAsync(getResponse);

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ArgumentException>()
                .WithMessage("Certificate is not eligible for print request");
        }
    }
}
