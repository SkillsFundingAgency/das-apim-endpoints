using System.Net;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Moq;
using SFA.DAS.Aodp.Application.Constants;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Models.DfeSignIn;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Review
{
    [TestFixture]
    public class GetApplicationsForReviewQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private Mock<IDfeUsersService> _dfeUsersServiceMock;
        private GetApplicationsForReviewQueryHandler _handler;

        private readonly string errorMessage = "error text";
        private readonly string qfauUkprn = "12345678";

        private List<User> _users;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder());

            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _dfeUsersServiceMock = _fixture.Freeze<Mock<IDfeUsersService>>();

            _fixture.Register(() => new DfeSignInApiConfiguration { QfauUkprn = qfauUkprn });

            _users = new List<User>
            {
                new User
                {
                    Email = "reviewer.one@test.com",
                    FirstName = "Reviewer",
                    LastName = "One"
                },
                new User
                {
                    Email = "reviewer.two@test.com",
                    FirstName = "Reviewer",
                    LastName = "Two"
                }
            };

            _handler = _fixture.Create<GetApplicationsForReviewQueryHandler>();
        }

        [Test]
        public async Task Then_The_ApiClient_Is_Called_And_Applications_Are_Returned_And_Reviewers_Are_Mapped()
        {
            // arrange
            var query = _fixture.Create<GetApplicationsForReviewQuery>();

            var responseBody = _fixture.Create<GetApplicationsForReviewQueryResponse>();

            var apiResponse = new ApiResponse<GetApplicationsForReviewQueryResponse>(
                responseBody,
                HttpStatusCode.OK,
                string.Empty);

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<GetApplicationsForReviewQueryResponse>(It.IsAny<IPostApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            _dfeUsersServiceMock
                .Setup(x => x.GetUsersByRoleAsync(qfauUkprn, DfeRoles.Reviewer, default))
                .ReturnsAsync(_users);

            // act
            var result = await _handler.Handle(query, CancellationToken.None);

            // assert
            Assert.Multiple(() =>
            {
                _apiClientMock.Verify(
                    x => x.PostWithResponseCode<GetApplicationsForReviewQueryResponse>(It.IsAny<IPostApiRequest>(), true),
                    Times.Once);

                _dfeUsersServiceMock.Verify(
                    x => x.GetUsersByRoleAsync(qfauUkprn, DfeRoles.Reviewer, default),
                    Times.Once);

                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.Not.Null);

                Assert.That(result.Value, Is.EqualTo(responseBody));

                Assert.That(result.Value.AvailableReviewers, Is.Not.Null);
                Assert.That(result.Value.AvailableReviewers.Count, Is.EqualTo(_users.Count));

                var expectedEmails = _users.Select(u => u.Email).ToHashSet();
                Assert.That(result.Value.AvailableReviewers.All(r => expectedEmails.Contains(r.Email)), Is.True);

                Assert.That(result.ErrorMessage, Is.Null.Or.Empty);
            });
        }

        [Test]
        public async Task Then_When_The_ApiClient_Throws_An_Exception_Failure_Is_Returned()
        {
            // arrange
            var query = _fixture.Create<GetApplicationsForReviewQuery>();

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<GetApplicationsForReviewQueryResponse>(It.IsAny<IPostApiRequest>(), true))
                .ThrowsAsync(new Exception(errorMessage));

            // act
            var result = await _handler.Handle(query, CancellationToken.None);

            // assert
            Assert.Multiple(() =>
            {
                _apiClientMock.Verify(
                    x => x.PostWithResponseCode<GetApplicationsForReviewQueryResponse>(It.IsAny<IPostApiRequest>(), true),
                    Times.Once);

                // If the API call fails, we shouldn't try to fetch users
                _dfeUsersServiceMock.Verify(
                    x => x.GetUsersByRoleAsync(It.IsAny<string>(), It.IsAny<string>(), default),
                    Times.Never);

                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(errorMessage));
            });
        }

        [Test]
        public async Task Then_When_The_DfeUsersService_Throws_An_Exception_Failure_Is_Returned()
        {
            // arrange
            var query = _fixture.Create<GetApplicationsForReviewQuery>();

            var responseBody = _fixture.Create<GetApplicationsForReviewQueryResponse>();

            var apiResponse = new ApiResponse<GetApplicationsForReviewQueryResponse>(
                responseBody,
                HttpStatusCode.OK,
                string.Empty);

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<GetApplicationsForReviewQueryResponse>(It.IsAny<IPostApiRequest>(), true))
                .ReturnsAsync(apiResponse);

            _dfeUsersServiceMock
                .Setup(x => x.GetUsersByRoleAsync(qfauUkprn, DfeRoles.Reviewer, default))
                .ThrowsAsync(new Exception(errorMessage));

            // act
            var result = await _handler.Handle(query, CancellationToken.None);

            // assert
            Assert.Multiple(() =>
            {
                _apiClientMock.Verify(
                    x => x.PostWithResponseCode<GetApplicationsForReviewQueryResponse>(It.IsAny<IPostApiRequest>(), true),
                    Times.Once);

                _dfeUsersServiceMock.Verify(
                    x => x.GetUsersByRoleAsync(qfauUkprn, DfeRoles.Reviewer, default),
                    Times.Once);

                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(errorMessage));
            });
        }

        public class DateOnlySpecimenBuilder : ISpecimenBuilder
        {
            public object Create(object request, ISpecimenContext context)
            {
                if (request is Type type)
                {
                    if (type == typeof(DateOnly))
                        return DateOnly.FromDateTime(DateTime.UtcNow.Date);

                    if (type == typeof(DateOnly?))
                        return (DateOnly?)DateOnly.FromDateTime(DateTime.UtcNow.Date);
                }

                return new NoSpecimen();
            }
        }
    }

   
}
