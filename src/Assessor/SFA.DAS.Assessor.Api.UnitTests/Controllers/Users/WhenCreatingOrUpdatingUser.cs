using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Assessor.Api.Controllers;
using SFA.DAS.Assessor.Api.Models.Users;
using SFA.DAS.Assessor.Application.Commands.CreateOrUpdateUser;

namespace SFA.DAS.Assessor.Api.UnitTests.Controllers.Users
{
    public class WhenCreatingOrUpdatingUser
    {
        private Mock<IMediator> _mediator;
        private UsersController _sut;
        private CreateOrUpdateUserRequest _request;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();

            _sut = new UsersController(
                _mediator.Object,
                Mock.Of<ILogger<UsersController>>());

            _request = new CreateOrUpdateUserRequest
            {
                DfeSignInUserId = "MixedCase-Subject",
                EmailAddress = "user@example.com",
                PhoneNumber = "07123456789"
            };
        }

        [TestCase("07123456789")]
        [TestCase(null)]
        public async Task Then_The_Command_Is_Mapped_And_UserId_Is_Returned(
            string phoneNumber)
        {
            using var cancellation = new CancellationTokenSource();

            _request.PhoneNumber = phoneNumber;
            var userId = Guid.NewGuid();

            _mediator
                .Setup(x => x.Send(
                    It.Is<CreateOrUpdateUserCommand>(command =>
                        command.DfeSignInUserId == _request.DfeSignInUserId &&
                        command.EmailAddress == _request.EmailAddress &&
                        command.PhoneNumber == phoneNumber),
                    cancellation.Token))
                .ReturnsAsync(new CreateOrUpdateUserResult
                {
                    UserId = userId
                });

            var actual = await _sut.CreateOrUpdateUser(
                _request,
                cancellation.Token);

            var response = actual.Should()
                .BeOfType<OkObjectResult>().Subject;

            response.Value.Should()
                .BeOfType<CreateOrUpdateUserResponse>()
                .Which.UserId.Should().Be(userId);

            _mediator.Verify(
                x => x.Send(
                    It.IsAny<CreateOrUpdateUserCommand>(),
                    cancellation.Token),
                Times.Once);
        }

        [Test]
        public async Task Then_Inner_Validation_Errors_Are_Returned_As_Original_Json()
        {
            const string error =
                "{\"errors\":[{\"propertyName\":\"EmailAddress\"," +
                "\"errorMessage\":\"EmailAddress must not be empty\"}]}";

            _mediator
                .Setup(x => x.Send(
                    It.IsAny<CreateOrUpdateUserCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiResponseException(
                    HttpStatusCode.BadRequest,
                    error));

            var actual = await _sut.CreateOrUpdateUser(_request);

            var response = actual.Should()
                .BeOfType<ContentResult>().Subject;

            response.StatusCode.Should()
                .Be(StatusCodes.Status400BadRequest);

            response.ContentType.Should().Be("application/json");
            response.Content.Should().Be(error);
        }

        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.NotFound)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        public async Task Then_Other_Inner_Failures_Return_InternalServerError(
            HttpStatusCode status)
        {
            _mediator
                .Setup(x => x.Send(
                    It.IsAny<CreateOrUpdateUserCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiResponseException(
                    status,
                    "Inner API failure"));

            var actual = await _sut.CreateOrUpdateUser(_request);

            actual.Should()
                .BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should()
                .Be(StatusCodes.Status500InternalServerError);
        }

        [Test]
        public async Task Then_An_Unexpected_Exception_Returns_InternalServerError()
        {
            _mediator
                .Setup(x => x.Send(
                    It.IsAny<CreateOrUpdateUserCommand>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(
                    "Unexpected failure"));

            var actual = await _sut.CreateOrUpdateUser(_request);

            actual.Should()
                .BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should()
                .Be(StatusCodes.Status500InternalServerError);
        }
    }
}