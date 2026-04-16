using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications
{
    [TestFixture]
    public class GetQualificationOutputFileLogQueryHandlerTests
    {
        private IFixture _fixture = null!;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock = null!;

        private const string NoLogsMessage = "No output file logs found.";
        private const string GenericError = "Something went wrong";

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_And_Logs_Are_Returned_Successfully()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationOutputFileLogQuery>();

            var logsResponse = _fixture.Build<GetQualificationOutputFileLogResponse>()
                .With(r => r.OutputFileLogs, _fixture.CreateMany<GetQualificationOutputFileLogResponse.QualificationOutputFileLog>(2).ToList())
                .Create();

            var apiResponse = _fixture.Build<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>()
                .With(r => r.Success, true)
                .With(r => r.Value, logsResponse)
                .Create();

            _apiClientMock
                .Setup(x => x.Get<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>(It.IsAny<GetQualificationOutputFileLogApiRequest>()))
                .ReturnsAsync(apiResponse);

            var handler = new GetQualificationOutputFileLogQueryHandler(_apiClientMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(
                x => x.Get<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>(It.IsAny<GetQualificationOutputFileLogApiRequest>()),
                Times.Once);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value!.OutputFileLogs, Is.Not.Null.And.Not.Empty);
                Assert.That(result.Value.OutputFileLogs.Count(), Is.EqualTo(2));
            });
        }

        [Test]
        public async Task Then_The_Api_Returns_No_Logs_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationOutputFileLogQuery>();

            var apiResponse = _fixture.Build<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>()
                .With(r => r.Success, false)
                .With(r => r.ErrorMessage, NoLogsMessage)
                .With(r => r.Value, (GetQualificationOutputFileLogResponse?)null)
                .Create();

            _apiClientMock
                .Setup(x => x.Get<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>(It.IsAny<GetQualificationOutputFileLogApiRequest>()))
                .ReturnsAsync(apiResponse);

            var handler = new GetQualificationOutputFileLogQueryHandler(_apiClientMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(
                x => x.Get<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>(It.IsAny<GetQualificationOutputFileLogApiRequest>()),
                Times.Once);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(NoLogsMessage));
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public async Task Then_The_Api_Throws_Exception_And_Handler_Returns_Failure()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationOutputFileLogQuery>();

            _apiClientMock
                .Setup(x => x.Get<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>(It.IsAny<GetQualificationOutputFileLogApiRequest>()))
                .ThrowsAsync(new Exception(GenericError));

            var handler = new GetQualificationOutputFileLogQueryHandler(_apiClientMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(
                x => x.Get<BaseMediatrResponse<GetQualificationOutputFileLogResponse>>(It.IsAny<GetQualificationOutputFileLogApiRequest>()),
                Times.Once);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(GenericError));
                Assert.That(result.Value, Is.Not.Null);
            });
        }
    }
}
