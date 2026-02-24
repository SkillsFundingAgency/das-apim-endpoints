using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Queries.Application.Review;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Models.DfeSignIn;
using System.Net;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Application.Review
{
    [TestFixture]
    public class GetApplicationForReviewByIdQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetApplicationForReviewByIdQueryHandler _handler;
        private Mock<IDfeUsersService> _dfeUsersServiceMock;
        private Mock<IOptions<DfeSignInApiConfiguration>> _cfgMock;

        private readonly Guid applicationReviewId = new();
        private readonly string errorMessage = "error text";
        private List<User> _users;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Customizations.Add(new DateOnlySpecimenBuilder()); 

            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _dfeUsersServiceMock = _fixture.Freeze<Mock<IDfeUsersService>>();

           _users = new List<User>
            {
                new User { Email = "a@test.com", FirstName = "A", LastName = "One" },
                new User { Email = "b@test.com", FirstName = "B", LastName = "Two" }
            };

            _dfeUsersServiceMock
                .Setup(x => x.GetUsersByRoleAsync(It.IsAny<string>(), It.IsAny<string>(), default))
                .ReturnsAsync(_users);

            _cfgMock = _fixture.Freeze<Mock<IOptions<DfeSignInApiConfiguration>>>();
            _cfgMock.Setup(x => x.Value).Returns(new DfeSignInApiConfiguration
            {
                QfauUkprn = "12345678" 
            });

            _handler = _fixture.Create<GetApplicationForReviewByIdQueryHandler>();
        }

        [Test]
        public async Task Then_The_ApiClient_Is_Called_And_ApplicationReview_Is_Returned()
        {
            //arrange
            var query = _fixture.Build<GetApplicationForReviewByIdQuery>()
                .With(x => x.ApplicationReviewId, applicationReviewId)
                .Create();

            var responseBody = _fixture.Create<GetApplicationForReviewByIdQueryResponse>();

            var apiResponse = new ApiResponse<GetApplicationForReviewByIdQueryResponse>(
                responseBody,
                HttpStatusCode.OK,
                string.Empty);

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetApplicationForReviewByIdQueryResponse>(It.IsAny<IGetApiRequest>()))
                .ReturnsAsync(apiResponse);

            //act
            var result = await _handler.Handle(query, CancellationToken.None);

            //asset
            Assert.Multiple(() =>
            {
                _apiClientMock.Verify(
                    x => x.GetWithResponseCode<GetApplicationForReviewByIdQueryResponse>(It.IsAny<IGetApiRequest>()),
                    Times.Once);

                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value, Is.EqualTo(responseBody));
                Assert.That(result.ErrorMessage, Is.Null.Or.Empty);

                var firstNames = _users.Select(u => u.FirstName).ToHashSet();

                Assert.That(result.Value.AvailableReviewers, Is.Not.Null);
                Assert.That(result.Value.AvailableReviewers, Has.Count.EqualTo(2));
                Assert.That(result.Value.AvailableReviewers.All(r => firstNames.Contains(r.FirstName)), Is.True);

            });
        }

        [Test]
        public async Task Then_When_The_ApiClient_Throws_An_Exception_Failure_Is_Returned()
        {
            //arrange
            var query = _fixture.Build<GetApplicationForReviewByIdQuery>()
                .With(x => x.ApplicationReviewId, applicationReviewId)
                .Create();

            _apiClientMock
                .Setup(x => x.GetWithResponseCode<GetApplicationForReviewByIdQueryResponse>(It.IsAny<IGetApiRequest>()))
                .ThrowsAsync(new Exception(errorMessage));

            //act
            var result = await _handler.Handle(query, CancellationToken.None);

            //asset
            Assert.Multiple(() =>
            {
                _apiClientMock.Verify(
                    x => x.GetWithResponseCode<GetApplicationForReviewByIdQueryResponse>(It.IsAny<IGetApiRequest>()),
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
