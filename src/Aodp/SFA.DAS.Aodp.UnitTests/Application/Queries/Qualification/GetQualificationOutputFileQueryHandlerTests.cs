using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications
{
    [TestFixture]
    public class GetQualificationOutputFileQueryHandlerTests
    {
        private IFixture _fixture = null!;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock = null!;

        // Reused constants
        private const string ContentTypeZip = "application/zip";
        private const string GenericError = "Export file not returned.";
        private const string TestZipName = "qualifications_export.zip";

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_And_OutputFile_Is_Returned()
        {
            var query = _fixture.Create<GetQualificationOutputFileQuery>();

            var apiResponse = _fixture.Build<BaseMediatrResponse<GetQualificationOutputFileResponse>>()
                .With(r => r.Success, true)
                .With(r => r.Value, new GetQualificationOutputFileResponse
                {
                    FileName = TestZipName,
                    ZipFileContent = new byte[] { 1, 2, 3 },
                    ContentType = ContentTypeZip
                })
                .Create();

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                    It.Is<GetQualificationOutputFileApiRequest>(r => true), true))
                .ReturnsAsync(
                    new ApiResponse<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                        apiResponse,
                        HttpStatusCode.OK,
                        string.Empty));

            var handler = new GetQualificationOutputFileQueryHandler(_apiClientMock.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            _apiClientMock.Verify(
                x => x.PostWithResponseCode<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                    It.Is<GetQualificationOutputFileApiRequest>(r => true), true),
                Times.Once);

            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.True);
                Assert.That(result.ErrorCode, Is.Null);
                Assert.That(result.Value, Is.Not.Null);
                Assert.That(result.Value!.FileName, Is.EqualTo(apiResponse.Value!.FileName));
                Assert.That(result.Value.ZipFileContent, Is.Not.Null.And.Not.Empty);
                Assert.That(result.Value.ContentType, Is.EqualTo(ContentTypeZip));
            });
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned_When_Success_Is_False()
        {
            var query = _fixture.Create<GetQualificationOutputFileQuery>();

            var apiResponse = _fixture.Build<BaseMediatrResponse<GetQualificationOutputFileResponse>>()
                .With(r => r.Success, false)
                .With(r => r.ErrorMessage, GenericError)
                .With(r => r.ErrorCode, ErrorCodes.UnexpectedError)
                .With(r => r.Value, (GetQualificationOutputFileResponse?)null)
                .Create();

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                    It.Is<GetQualificationOutputFileApiRequest>(r => true), true))
                .ReturnsAsync(
                    new ApiResponse<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                        apiResponse,
                        HttpStatusCode.OK,
                        string.Empty));

            var handler = new GetQualificationOutputFileQueryHandler(_apiClientMock.Object);
            var result = await handler.Handle(query, CancellationToken.None);

            Assert.Multiple(() =>
            {
                _apiClientMock.Verify(
                    x => x.PostWithResponseCode<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                        It.IsAny<GetQualificationOutputFileApiRequest>(), true),
                    Times.Once);

                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(GenericError));
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCodes.UnexpectedError));
                Assert.That(result.Value, Is.Not.Null);
            });
        }

        [Test]
        public async Task Then_The_Api_Returns_Empty_ZipFile_And_Is_Treated_As_NoData()
        {
            var query = _fixture.Create<GetQualificationOutputFileQuery>();

            var apiResponse = new BaseMediatrResponse<GetQualificationOutputFileResponse>
            {
                Success = true,
                Value = new GetQualificationOutputFileResponse
                {
                    FileName = string.Empty,
                    ZipFileContent = Array.Empty<byte>(), 
                    ContentType = "application/zip"
                },
                ErrorMessage = GenericError,
                ErrorCode = ErrorCodes.NoData
            };

            _apiClientMock
                .Setup(x => x.PostWithResponseCode<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                    It.Is<GetQualificationOutputFileApiRequest>(r => true), true))
                .ReturnsAsync(
                    new ApiResponse<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                        apiResponse,
                        HttpStatusCode.OK,
                        string.Empty));

            var handler = new GetQualificationOutputFileQueryHandler(_apiClientMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCodes.NoData));
                Assert.That(result.ErrorMessage, Is.EqualTo(GenericError));
                Assert.That(result.Value, Is.Not.Null); 
            });
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
        {
            // Arrange
            var query = _fixture.Create<GetQualificationOutputFileQuery>();
            var exceptionMessage = GenericError;

            _apiClientMock
                .Setup(x => x.PostWithResponseCode <BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                    It.Is<GetQualificationOutputFileApiRequest>(r => true), true))
                .ThrowsAsync(new Exception(exceptionMessage));

            var handler = new GetQualificationOutputFileQueryHandler(_apiClientMock.Object);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Multiple(() =>
            {
                _apiClientMock.Verify(
                    x => x.PostWithResponseCode<BaseMediatrResponse<GetQualificationOutputFileResponse>>(
                        It.IsAny<GetQualificationOutputFileApiRequest>(), true),
                    Times.Once);
                Assert.That(result.Success, Is.False);
                Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
                Assert.That(result.ErrorCode, Is.EqualTo(ErrorCodes.UnexpectedError));
                Assert.That(result.Value, Is.Not.Null);
            });
        }
    }
}
