using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.CreateOrUpdateUser
{
    public class WhenHandlingCreateOrUpdateUserCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_User_Is_Created_Or_Updated_Successfully(
            CreateOrUpdateUserCommand command,
            CreateOrUpdateUserResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateOrUpdateUserCommandHandler handler)
        {
            // Arrange
            var apiResponse = new ApiResponse<CreateOrUpdateUserResponse>(
                apiResponseBody, HttpStatusCode.OK, string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateOrUpdateUserResponse>(
                    It.Is<PostUsersApiRequest>(r =>
                        ((CreateOrUpdateUserRequest)r.Data).GovUkIdentifier == command.GovUkIdentifier &&
                        ((CreateOrUpdateUserRequest)r.Data).EmailAddress == command.EmailAddress &&
                        ((CreateOrUpdateUserRequest)r.Data).PhoneNumber == command.PhoneNumber), true))
                .ReturnsAsync(apiResponse);

            // Act
            var actual = await handler.Handle(command, CancellationToken.None);

            // Assert
            actual.Should().BeEquivalentTo(apiResponseBody);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Correctly_Constructed(
            CreateOrUpdateUserCommand command,
            CreateOrUpdateUserResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateOrUpdateUserCommandHandler handler)
        {
            // Arrange
            var response = new ApiResponse<CreateOrUpdateUserResponse>(apiResponseBody, HttpStatusCode.OK, string.Empty);
            IPostApiRequest capturedRequest = null;

            mockDigitalCertificatesApiClient
                .Setup(c => c.PostWithResponseCode<CreateOrUpdateUserResponse>(
                    It.IsAny<IPostApiRequest>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest, bool>((req, includeResponse) =>
                {
                    capturedRequest = req;
                })
                .ReturnsAsync(response);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            capturedRequest.Should().BeOfType<PostUsersApiRequest>();
            capturedRequest!.PostUrl.Should().Be("api/users");

            var data = ((PostUsersApiRequest)capturedRequest).Data.Should().BeOfType<CreateOrUpdateUserRequest>().Subject;
            data.GovUkIdentifier.Should().Be(command.GovUkIdentifier);
            data.EmailAddress.Should().Be(command.EmailAddress);
            data.PhoneNumber.Should().Be(command.PhoneNumber);
        }


        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateOrUpdateUserCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateOrUpdateUserCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<CreateOrUpdateUserResponse>(
                    It.IsAny<PostUsersApiRequest>(), true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
