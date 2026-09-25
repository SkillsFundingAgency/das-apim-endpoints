using System;
using System.Linq;
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
using SFA.DAS.DigitalCertificates.Contracts.ApiRequests;
using SFA.DAS.DigitalCertificates.Contracts.ApiResponses;
using SFA.DAS.DigitalCertificates.Contracts.Client;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Commands.UpdateUserIdentity
{
    public class WhenHandlingUpdateUserIdentityCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_User_Identity_Is_Updated_Successfully(
            UpdateUserIdentityCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UpdateUserIdentityCommandHandler _sut)
        {
            var apiResponse = new ApiResponse<object>(
                null,
                HttpStatusCode.OK,
                string.Empty);

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostUsersByUserIdIdentityApiRequest>(),
                    false))
                .ReturnsAsync(apiResponse);

            var actual = await _sut.Handle(command, CancellationToken.None);

            actual.Should().Be(Unit.Value);

            mockDigitalCertificatesApiClient.Verify(client =>
                client.PostWithResponseCode<object>(
                    It.Is<PostUsersByUserIdIdentityApiRequest>(r =>
                        r.PostUrl == $"api/users/{command.UserId}/identity" &&
                        ((UpdateUserIdentityRequest)r.Data).DateOfBirth == command.DateOfBirth),
                    false),
                Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Correctly_Constructed(
            UpdateUserIdentityCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UpdateUserIdentityCommandHandler _sut)
        {
            var response = new ApiResponse<object>(
                null,
                HttpStatusCode.OK,
                string.Empty);

            IPostApiRequest capturedRequest = null;

            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<IPostApiRequest>(),
                    It.IsAny<bool>()))
                .Callback<IPostApiRequest, bool>((request, _) =>
                {
                    capturedRequest = request;
                })
                .ReturnsAsync(response);

            await _sut.Handle(command, CancellationToken.None);

            capturedRequest.Should().BeOfType<PostUsersByUserIdIdentityApiRequest>();
            capturedRequest!.PostUrl.Should().Be($"api/users/{command.UserId}/identity");

            var data = ((PostUsersByUserIdIdentityApiRequest)capturedRequest).Data.Should().BeOfType<UpdateUserIdentityRequest>().Subject;
            data.DateOfBirth.Should().Be(command.DateOfBirth);
            data.Names.Should().BeEquivalentTo(command.Names.Select(n => new NameRequest
            {
                UserIdentityId = n.UserIdentityId,
                ValidSince = n.ValidSince,
                ValidUntil = n.ValidUntil,
                FamilyName = n.FamilyName,
                GivenNames = n.GivenNames
            }));
        }

        [Test, MoqAutoData]
        public async Task Then_Exception_Is_Thrown_If_Api_Call_Fails(
            UpdateUserIdentityCommand command,
            [Frozen] Mock<IDigitalCertificatesApiClient<DigitalCertificatesApiConfiguration>> mockDigitalCertificatesApiClient,
            UpdateUserIdentityCommandHandler _sut)
        {
            mockDigitalCertificatesApiClient
                .Setup(client => client.PostWithResponseCode<object>(
                    It.IsAny<PostUsersByUserIdIdentityApiRequest>(),
                    false))
                .ThrowsAsync(new ApiResponseException(HttpStatusCode.BadRequest, "Bad request"));

            Func<Task> act = async () => await _sut.Handle(command, CancellationToken.None);

            await act.Should().ThrowAsync<ApiResponseException>()
                .Where(e => e.Status == HttpStatusCode.BadRequest);
        }
    }
}
