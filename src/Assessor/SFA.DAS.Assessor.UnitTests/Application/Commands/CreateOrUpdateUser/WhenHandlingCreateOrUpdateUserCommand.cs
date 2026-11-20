using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;
using SFA.DAS.Assessor.Contracts.ApiRequests;
using SFA.DAS.Assessor.Contracts.ApiResponses;
using SFA.DAS.Assessor.Contracts.Client;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessor.UnitTests.Application.Commands.CreateOrUpdateUser
{
    public class WhenHandlingCreateOrUpdateUserCommand
    {
        private Mock<IAssessorApiClient<AssessorApiConfiguration>> _apiClient;
        private CreateOrUpdateUserCommandHandler _sut;
        private CreateOrUpdateUserCommand _command;

        [SetUp]
        public void SetUp()
        {
            _apiClient =
                new Mock<IAssessorApiClient<AssessorApiConfiguration>>();

            _sut = new CreateOrUpdateUserCommandHandler(_apiClient.Object);

            _command = new CreateOrUpdateUserCommand
            {
                DfeSignInUserId = "MixedCase-Subject",
                EmailAddress = "user@example.com",
                PhoneNumber = "07123456789"
            };
        }

        [TestCase("07123456789")]
        [TestCase(null)]
        public async Task Then_The_Request_Is_Mapped_And_UserId_Is_Returned(
            string phoneNumber)
        {
            _command.PhoneNumber = phoneNumber;

            var userId = Guid.NewGuid();
            IPostApiRequest capturedRequest = null;

            _apiClient
                .Setup(x => x.PostWithResponseCode<CreateOrUpdateUserResponse>(
                    It.IsAny<IPostApiRequest>(), true))
                .Callback<IPostApiRequest, bool>((request, _) =>
                    capturedRequest = request)
                .ReturnsAsync(new ApiResponse<CreateOrUpdateUserResponse>(
                    new CreateOrUpdateUserResponse
                    {
                        UserId = userId
                    },
                    HttpStatusCode.OK,
                    string.Empty));

            var actual = await _sut.Handle(
                _command,
                CancellationToken.None);

            actual.UserId.Should().Be(userId);

            var request = capturedRequest.Should()
                .BeOfType<PostUsersApiRequest>().Subject;

            request.PostUrl.Should().Be("api/users");

            var data = request.Data.Should()
                .BeOfType<CreateOrUpdateUserRequest>().Subject;

            data.DfeSignInUserId.Should().Be(_command.DfeSignInUserId);
            data.EmailAddress.Should().Be(_command.EmailAddress);
            data.PhoneNumber.Should().Be(phoneNumber);

            _apiClient.Verify(
                x => x.PostWithResponseCode<CreateOrUpdateUserResponse>(
                    It.IsAny<IPostApiRequest>(), true),
                Times.Once);
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        public async Task Then_An_Unsuccessful_Response_Throws_With_Status_And_Error(
            HttpStatusCode status)
        {
            const string error = "{\"errors\":[\"Request failed\"]}";

            _apiClient
                .Setup(x => x.PostWithResponseCode<CreateOrUpdateUserResponse>(
                    It.IsAny<IPostApiRequest>(), true))
                .ReturnsAsync(new ApiResponse<CreateOrUpdateUserResponse>(
                    null,
                    status,
                    error));

            Func<Task> act = () => _sut.Handle(
                _command,
                CancellationToken.None);

            await act.Should()
                .ThrowAsync<ApiResponseException>()
                .Where(x => x.Status == status && x.Error == error);
        }

        [Test]
        public async Task Then_A_Successful_Response_Without_A_Body_Is_Rejected()
        {
            _apiClient
                .Setup(x => x.PostWithResponseCode<CreateOrUpdateUserResponse>(
                    It.IsAny<IPostApiRequest>(), true))
                .ReturnsAsync(new ApiResponse<CreateOrUpdateUserResponse>(
                    null,
                    HttpStatusCode.OK,
                    string.Empty));

            Func<Task> act = () => _sut.Handle(
                _command,
                CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task Then_A_Client_Exception_Is_Propagated()
        {
            var exception = new HttpRequestException(
                "The inner API could not be reached.");

            _apiClient
                .Setup(x => x.PostWithResponseCode<CreateOrUpdateUserResponse>(
                    It.IsAny<IPostApiRequest>(), true))
                .ThrowsAsync(exception);

            Func<Task> act = () => _sut.Handle(
                _command,
                CancellationToken.None);

            var actual = await act.Should()
                .ThrowAsync<HttpRequestException>();

            actual.Which.Should().BeSameAs(exception);
        }
    }
}