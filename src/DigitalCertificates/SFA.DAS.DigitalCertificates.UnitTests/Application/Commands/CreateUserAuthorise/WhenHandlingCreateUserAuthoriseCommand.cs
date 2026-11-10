using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateUserAuthorise;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateUserAuthorise
{
    public class WhenHandlingCreateUserAuthoriseCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Authorise_Request_Is_Forwarded_To_Inner_Api(
            CreateUserAuthoriseCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserAuthoriseCommandHandler handler)
        {
            // Arrange
            var apiResponse = new ApiResponse<object>(new object(), HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostUsersByUserIdAuthoriseApiRequest>(), false))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<object>(
                    It.Is<PostUsersByUserIdAuthoriseApiRequest>(r =>
                        ((CreateUserAuthorisationRequest)r.Data).Uln == command.Uln &&
                        r.PostUrl == $"api/users/{command.UserId}/authorise"), false), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateUserAuthoriseCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateUserAuthoriseCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostUsersByUserIdAuthoriseApiRequest>(), false))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<object>(It.IsAny<PostUsersByUserIdAuthoriseApiRequest>(), false), Times.Once);
        }
    }
}
