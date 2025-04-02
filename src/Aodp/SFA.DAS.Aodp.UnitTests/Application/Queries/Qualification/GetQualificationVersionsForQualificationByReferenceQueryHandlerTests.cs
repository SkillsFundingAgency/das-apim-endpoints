using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications
{
    [TestFixture]
    public class GetQualificationVersionsForQualificationByReferenceQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetQualificationVersionsForQualificationByReferenceQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = new GetQualificationVersionsForQualificationByReferenceQueryHandler(_apiClientMock.Object);
        }

        [Test]
        public async Task Handle_ReturnsSuccessResponse_WhenApiCallIsSuccessful()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationVersionsForQualificationByReferenceQuery>();
            var apiResponse = _fixture.Create<GetQualificationVersionsForQualificationByReferenceQueryResponse>();
            var apiResult = new ApiResponse<GetQualificationVersionsForQualificationByReferenceQueryResponse>(
                apiResponse,
                System.Net.HttpStatusCode.OK,
                string.Empty,
                new Dictionary<string, IEnumerable<string>>()
            );

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetQualificationVersionsForQualificationByReferenceQueryResponse>(It.IsAny<GetQualificationVersionsForQualificationByReferenceApiRequest>()))
                          .ReturnsAsync(apiResult);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value, Is.EqualTo(apiResponse));
        }

        [Test]
        public async Task Handle_ReturnsErrorResponse_WhenApiCallFails()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationVersionsForQualificationByReferenceQuery>();
            var exceptionMessage = "API call failed";

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetQualificationVersionsForQualificationByReferenceQueryResponse>(It.IsAny<GetQualificationVersionsForQualificationByReferenceApiRequest>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }
}
