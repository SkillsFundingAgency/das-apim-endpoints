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
using SFA.DAS.Assessor.Application.Queries.GetUser;
using SFA.DAS.Assessor.Models;

namespace SFA.DAS.Assessor.Api.UnitTests.Controllers.Users
{
    public class WhenGettingUser
    {
        private Mock<IMediator> _mediator;
        private UsersController _sut;

        [SetUp]
        public void SetUp()
        {
            _mediator = new Mock<IMediator>();

            _sut = new UsersController(
                _mediator.Object,
                Mock.Of<ILogger<UsersController>>());
        }

        [TestCase("07123456789", true)]
        [TestCase(null, false)]
        public async Task Then_The_Query_Is_Sent_And_All_User_Fields_Are_Returned(
            string phoneNumber,
            bool isLocked)
        {
            using var cancellation = new CancellationTokenSource();

            var user = new User
            {
                Id = Guid.NewGuid(),
                DfeSignInUserId = "MixedCase-Subject",
                EmailAddress = "user@example.com",
                PhoneNumber = phoneNumber,
                LastLoginAt = phoneNumber == null
                    ? (DateTime?)null
                    : DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                IsLocked = isLocked
            };

            _mediator
                .Setup(x => x.Send(
                    It.Is<GetUserQuery>(query =>
                        query.DfeSignInUserId == user.DfeSignInUserId),
                    cancellation.Token))
                .ReturnsAsync(new GetUserResult
                {
                    User = user
                });

            var actual = await _sut.GetUser(
                user.DfeSignInUserId,
                cancellation.Token);

            var response = actual.Should()
                .BeOfType<OkObjectResult>().Subject;

            response.Value.Should()
                .BeOfType<GetUserResponse>()
                .Which.Should().BeEquivalentTo(user);

            _mediator.Verify(
                x => x.Send(
                    It.IsAny<GetUserQuery>(),
                    cancellation.Token),
                Times.Once);
        }

        [Test]
        public async Task Then_A_Missing_User_Returns_NotFound()
        {
            _mediator
                .Setup(x => x.Send(
                    It.IsAny<GetUserQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(new GetUserResult());

            var actual = await _sut.GetUser("missing-subject");

            actual.Should().BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task Then_Inner_Validation_Errors_Are_Returned_As_Original_Json()
        {
            const string error =
                "{\"errors\":[{\"propertyName\":\"DfeSignInUserId\"," +
                "\"errorMessage\":\"Invalid identifier\"}]}";

            _mediator
                .Setup(x => x.Send(
                    It.IsAny<GetUserQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiResponseException(
                    HttpStatusCode.BadRequest,
                    error));

            var actual = await _sut.GetUser("subject");

            var response = actual.Should()
                .BeOfType<ContentResult>().Subject;

            response.StatusCode.Should()
                .Be(StatusCodes.Status400BadRequest);

            response.ContentType.Should().Be("application/json");
            response.Content.Should().Be(error);
        }

        [TestCase(HttpStatusCode.Unauthorized)]
        [TestCase(HttpStatusCode.Forbidden)]
        [TestCase(HttpStatusCode.InternalServerError)]
        [TestCase(HttpStatusCode.ServiceUnavailable)]
        public async Task Then_Other_Inner_Failures_Return_InternalServerError(
            HttpStatusCode status)
        {
            _mediator
                .Setup(x => x.Send(
                    It.IsAny<GetUserQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new ApiResponseException(
                    status,
                    "Inner API failure"));

            var actual = await _sut.GetUser("subject");

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
                    It.IsAny<GetUserQuery>(),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException(
                    "Unexpected failure"));

            var actual = await _sut.GetUser("subject");

            actual.Should()
                .BeOfType<StatusCodeResult>()
                .Which.StatusCode.Should()
                .Be(StatusCodes.Status500InternalServerError);
        }
    }
}