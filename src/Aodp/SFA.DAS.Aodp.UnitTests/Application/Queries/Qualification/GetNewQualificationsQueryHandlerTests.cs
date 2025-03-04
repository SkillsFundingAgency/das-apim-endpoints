using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.UnitTests.Application.Queries.Qualifications
{
    [TestFixture]
    public class GetNewQualificationsQueryHandlerTests
    {
        private IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private GetNewQualificationsQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetNewQualificationsQueryHandler>();
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_NewQualificationsData_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetNewQualificationsQuery>();
            var response = _fixture.Create<BaseMediatrResponse<GetNewQualificationsQueryResponse>>();
            response.Success = true;
            response.Value.Data = _fixture.CreateMany<NewQualification>(2).ToList();

            _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetNewQualificationsQueryResponse>>(It.IsAny<GetNewQualificationsApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetNewQualificationsQueryResponse>>(It.IsAny<GetNewQualificationsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value.Data.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetNewQualificationsQuery>();
            var baseResponse = new BaseMediatrResponse<GetNewQualificationsQueryResponse>
            {
                Success = false,
                Value = null,
                ErrorMessage = "No new qualifications found."
            };

            _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetNewQualificationsQueryResponse>>(It.IsAny<GetNewQualificationsApiRequest>()))
                          .ReturnsAsync(baseResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetNewQualificationsQueryResponse>>(It.IsAny<GetNewQualificationsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo("No new qualifications found."));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
        {
            // Arrange
            var query = _fixture.Create<GetNewQualificationsQuery>();
            var exceptionMessage = "An error occurred";
            _apiClientMock.Setup(x => x.Get<BaseMediatrResponse<GetNewQualificationsQueryResponse>>(It.IsAny<GetNewQualificationsApiRequest>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<BaseMediatrResponse<GetNewQualificationsQueryResponse>>(It.IsAny<GetNewQualificationsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }
    }
}
