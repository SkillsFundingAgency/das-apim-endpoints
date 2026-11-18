using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Assessor.Application.Queries.GetUser;
using SFA.DAS.Assessor.Contracts.ApiRequests;
using SFA.DAS.Assessor.Contracts.ApiResponses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Assessor.UnitTests.Application.Queries.GetUser
{
    public class WhenHandlingGetUserQuery
    {
        private Mock<IAssessorApiClient<AssessorApiConfiguration>> _apiClient;
        private GetUserQueryHandler _sut;
        private GetUserQuery _query;

        [SetUp]
        public void SetUp()
        {
            _apiClient =
                new Mock<IAssessorApiClient<AssessorApiConfiguration>>();

            _sut = new GetUserQueryHandler(_apiClient.Object);

            _query = new GetUserQuery
            {
                DfeSignInUserId = "MixedCase-Subject"
            };
        }

        [TestCase("07123456789", true)]
        [TestCase(null, false)]
        public async Task Then_The_Request_And_All_User_Fields_Are_Mapped(
            string phoneNumber,
            bool isLocked)
        {
            var user = new GetUserResponse
            {
                Id = Guid.NewGuid(),
                DfeSignInUserId = _query.DfeSignInUserId,
                EmailAddress = "user@example.com",
                PhoneNumber = phoneNumber,
                LastLoginAt = phoneNumber == null
                    ? (DateTime?)null
                    : DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                IsLocked = isLocked
            };

            IGetApiRequest capturedRequest = null;

            _apiClient
                .Setup(x => x.GetWithResponseCode<GetUserResponse>(
                    It.IsAny<IGetApiRequest>()))
                .Callback<IGetApiRequest>(request =>
                    capturedRequest = request)
                .ReturnsAsync(new ApiResponse<GetUserResponse>(
                    user,
                    HttpStatusCode.OK,
                    string.Empty));

            var actual = await _sut.Handle(
                _query,
                CancellationToken.None);

            var request = capturedRequest.Should()
                .BeOfType<GetUsersByDfeSignInUserIdApiRequest>().Subject;

            request.GetUrl.Should().Be("api/users/MixedCase-Subject");

            actual.User.Should().NotBeNull();
            actual.User.Should().BeEquivalentTo(user);

            _apiClient.Verify(
                x => x.GetWithResponseCode<GetUserResponse>(
                    It.IsAny<IGetApiRequest>()),
                Times.Once);
        }

        [TestCase(false)]
        [TestCase(true)]
        public async Task Then_NotFound_Returns_No_User(
            bool includeResponseBody)
        {
            var body = includeResponseBody
                ? new GetUserResponse { Id = Guid.NewGuid() }
                : null;

            _apiClient
                .Setup(x => x.GetWithResponseCode<GetUserResponse>(
                    It.IsAny<IGetApiRequest>()))
                .ReturnsAsync(new ApiResponse<GetUserResponse>(
                    body,
                    HttpStatusCode.NotFound,
                    string.Empty));

            var actual = await _sut.Handle(
                _query,
                CancellationToken.None);

            actual.User.Should().BeNull();
        }

        [TestCase(HttpStatusCode.BadRequest)]
        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        public async Task Then_Other_Unsuccessful_Responses_Throw_With_Status_And_Error(
            HttpStatusCode status)
        {
            const string error = "{\"errors\":[\"Request failed\"]}";

            _apiClient
                .Setup(x => x.GetWithResponseCode<GetUserResponse>(
                    It.IsAny<IGetApiRequest>()))
                .ReturnsAsync(new ApiResponse<GetUserResponse>(
                    null,
                    status,
                    error));

            Func<Task> act = () => _sut.Handle(
                _query,
                CancellationToken.None);

            await act.Should()
                .ThrowAsync<ApiResponseException>()
                .Where(x => x.Status == status && x.Error == error);
        }

        [Test]
        public async Task Then_A_Successful_Response_Without_A_Body_Is_Rejected()
        {
            _apiClient
                .Setup(x => x.GetWithResponseCode<GetUserResponse>(
                    It.IsAny<IGetApiRequest>()))
                .ReturnsAsync(new ApiResponse<GetUserResponse>(
                    null,
                    HttpStatusCode.OK,
                    string.Empty));

            Func<Task> act = () => _sut.Handle(
                _query,
                CancellationToken.None);

            await act.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task Then_A_Client_Exception_Is_Propagated()
        {
            var exception = new HttpRequestException(
                "The inner API could not be reached.");

            _apiClient
                .Setup(x => x.GetWithResponseCode<GetUserResponse>(
                    It.IsAny<IGetApiRequest>()))
                .ThrowsAsync(exception);

            Func<Task> act = () => _sut.Handle(
                _query,
                CancellationToken.None);

            var actual = await act.Should()
                .ThrowAsync<HttpRequestException>();

            actual.Which.Should().BeSameAs(exception);
        }
    }
}