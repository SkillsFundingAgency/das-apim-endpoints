using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NServiceBus.Features;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.AODP.Application.Queries.Qualifications;
using SFA.DAS.AODP.Domain.Qualifications.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications
{
    [TestFixture]
    public class GetQualificationDetailsWithVersionsQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetQualificationDetailWithVersionsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>()
      .ToList()
      .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetQualificationDetailWithVersionsQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_QualificationDetailsData_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationDetailWithVersionsQuery>();
            var response = _fixture.Create<GetQualificationDetailsQueryResponse>();

            _apiClientMock.Setup(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailWithVersionsApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailWithVersionsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value.Id, Is.EqualTo(response.Id));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationDetailWithVersionsQuery>();

            _apiClientMock.Setup(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailWithVersionsApiRequest>()))
                          .Returns(Task.FromResult<GetQualificationDetailsQueryResponse>(null));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailWithVersionsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo($"No details found for qualification reference: {query.QualificationReference}"));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationDetailWithVersionsQuery>();
            var exceptionMessage = "An error occurred";
            _apiClientMock.Setup(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailWithVersionsApiRequest>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetQualificationDetailsQueryResponse>(It.IsAny<GetQualificationDetailWithVersionsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }
}
