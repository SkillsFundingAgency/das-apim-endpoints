using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.DigitalCertificates.InnerApi.Requests.PostCreateOrUpdateUserRequest;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands
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
                .Setup(client => client.PostWithResponseCode<PostCreateOrUpdateUserRequestData, CreateOrUpdateUserResponse>(
                    It.Is<PostCreateOrUpdateUserRequest>(r =>
                        r.Data.GovUkIdentifier == command.GovUkIdentifier &&
                        r.Data.EmailAddress == command.EmailAddress &&
                        r.Data.PhoneNumber == command.PhoneNumber &&
                        r.Data.Names == command.Names &&
                        r.Data.DateOfBirth == command.DateOfBirth), true))
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
            PostCreateOrUpdateUserRequest capturedRequest = null;

            mockDigitalCertificatesApiClient
                .Setup(c => c.PostWithResponseCode<PostCreateOrUpdateUserRequestData, CreateOrUpdateUserResponse>(
                    It.IsAny<IPostApiRequest<PostCreateOrUpdateUserRequestData>>(), It.IsAny<bool>()))
                .Callback<IPostApiRequest<PostCreateOrUpdateUserRequestData>, bool>((req, includeResponse) =>
                {
                    capturedRequest = (PostCreateOrUpdateUserRequest)req;
                })
                .ReturnsAsync(response);

            // Act
            await handler.Handle(command, CancellationToken.None);

            // Assert
            capturedRequest.Should().NotBeNull();
            capturedRequest!.Data.GovUkIdentifier.Should().Be(command.GovUkIdentifier);
            capturedRequest!.Data.EmailAddress.Should().Be(command.EmailAddress);
            capturedRequest!.Data.PhoneNumber.Should().Be(command.PhoneNumber);
            capturedRequest!.Data.Names.Should().BeEquivalentTo(command.Names);
            capturedRequest!.Data.DateOfBirth.Should().Be(command.DateOfBirth);
        }


        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            CreateOrUpdateUserCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            CreateOrUpdateUserCommandHandler handler)
        {
            // Arrange
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostCreateOrUpdateUserRequestData, CreateOrUpdateUserResponse>(
                    It.IsAny<PostCreateOrUpdateUserRequest>(), true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            // Act & Assert
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
