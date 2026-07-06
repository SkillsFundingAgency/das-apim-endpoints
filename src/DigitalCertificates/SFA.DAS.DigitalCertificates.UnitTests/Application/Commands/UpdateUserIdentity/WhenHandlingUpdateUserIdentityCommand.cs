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
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.DigitalCertificates.Application.Commands.UpdateUserIdentity;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;
using SFA.DAS.DigitalCertificates.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.DigitalCertificates.InnerApi.Requests.PostUpdateUserIdentityRequest;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.UpdateUserIdentity
{
    public class WhenHandlingUpdateUserIdentityCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_User_Identity_Is_Updated_Successfully(
            UpdateUserIdentityCommand command,
            PostUpdateUserIdentityResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UpdateUserIdentityCommandHandler _sut)
        {
            var apiResponse = new ApiResponse<PostUpdateUserIdentityResponse>(
                apiResponseBody,
                HttpStatusCode.OK,
                string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostUpdateUserIdentityRequestData, PostUpdateUserIdentityResponse>(
                    It.Is<PostUpdateUserIdentityRequest>(r =>
                        r.PostUrl == $"api/users/{command.UserId}/identity" &&
                        r.Data.Names == command.Names &&
                        r.Data.DateOfBirth == command.DateOfBirth),
                    true))
                .ReturnsAsync(apiResponse);

            var actual = await _sut.Handle(command, CancellationToken.None);

            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<PostUpdateUserIdentityRequestData, PostUpdateUserIdentityResponse>(
                    It.Is<PostUpdateUserIdentityRequest>(r =>
                        r.PostUrl == $"api/users/{command.UserId}/identity" &&
                        r.Data.Names == command.Names &&
                        r.Data.DateOfBirth == command.DateOfBirth),
                    true),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Correctly_Constructed(
            UpdateUserIdentityCommand command,
            PostUpdateUserIdentityResponse apiResponseBody,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UpdateUserIdentityCommandHandler _sut)
        {
            var response = new ApiResponse<PostUpdateUserIdentityResponse>(
                apiResponseBody,
                HttpStatusCode.OK,
                string.Empty);

            PostUpdateUserIdentityRequest capturedRequest = null;

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostUpdateUserIdentityRequestData, PostUpdateUserIdentityResponse>(
                    It.IsAny<IPostApiRequest<PostUpdateUserIdentityRequestData>>(),
                    It.IsAny<bool>()))
                .Callback<IPostApiRequest<PostUpdateUserIdentityRequestData>, bool>((request, _) =>
                {
                    capturedRequest = (PostUpdateUserIdentityRequest)request;
                })
                .ReturnsAsync(response);

            await _sut.Handle(command, CancellationToken.None);

            capturedRequest.Should().NotBeNull();
            capturedRequest!.PostUrl.Should().Be($"api/users/{command.UserId}/identity");
            capturedRequest.Data.Names.Should().BeSameAs(command.Names);
            capturedRequest.Data.DateOfBirth.Should().Be(command.DateOfBirth);
        }

        [Test, MoqAutoData]
        public void Then_Exception_Is_Thrown_If_Api_Call_Fails(
            UpdateUserIdentityCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UpdateUserIdentityCommandHandler _sut)
        {
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<PostUpdateUserIdentityRequestData, PostUpdateUserIdentityResponse>(
                    It.IsAny<PostUpdateUserIdentityRequest>(),
                    true))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}