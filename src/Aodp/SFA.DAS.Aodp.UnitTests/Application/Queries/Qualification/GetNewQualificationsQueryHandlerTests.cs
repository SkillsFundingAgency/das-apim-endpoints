using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Qualifications;
using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.Aodp.Models;
using SFA.DAS.Aodp.Services;
using SFA.DAS.Apim.Shared.Interfaces;

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
            var response = _fixture.Create<GetNewQualificationsApiResponse>();            
            response.Data = _fixture.CreateMany<NewQualification>(2).ToList();

            _apiClientMock.Setup(x => x.Get<GetNewQualificationsApiResponse>(It.IsAny<GetNewQualificationsApiRequest>()))
                          .ReturnsAsync(response);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetNewQualificationsApiResponse>(It.IsAny<GetNewQualificationsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.True);
            Assert.That(result.Value.Data.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Failure_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetNewQualificationsQuery>();
            var baseResponse = new GetNewQualificationsApiResponse
            {
                Data = null
            };

            _apiClientMock.Setup(x => x.Get<GetNewQualificationsApiResponse>(It.IsAny<GetNewQualificationsApiRequest>()))
                          .ReturnsAsync(baseResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetNewQualificationsApiResponse>(It.IsAny<GetNewQualificationsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.False);
        }

        [Test]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_Exception_Is_Handled()
        {
            // Arrange
            var query = _fixture.Create<GetNewQualificationsQuery>();
            var exceptionMessage = "An error occurred";
            _apiClientMock.Setup(x => x.Get<GetNewQualificationsApiResponse>(It.IsAny<GetNewQualificationsApiRequest>()))
                          .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.Get<GetNewQualificationsApiResponse>(It.IsAny<GetNewQualificationsApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.EqualTo(exceptionMessage));
        }

        [Test]
        public async Task Handle_Passes_ProcessStatusFilter_And_AgeGroups_To_ApiRequest()
        {
            // Arrange
            var processStatuses = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
            var ageGroups = new List<AgeGroup> { AgeGroup.EighteenPlus, AgeGroup.NineteenPlus };

            GetNewQualificationsApiRequest? capturedRequest = null;

            _apiClientMock
                .Setup(x => x.Get<GetNewQualificationsApiResponse>(It.IsAny<GetNewQualificationsApiRequest>()))
                .Callback<IGetApiRequest>(req => capturedRequest = (GetNewQualificationsApiRequest)req)
                .ReturnsAsync(new GetNewQualificationsApiResponse
                {
                    Data = new List<NewQualification>()
                });

            var query = new GetNewQualificationsQuery
            {
                ProcessStatusFilter = processStatuses,
                AgeGroups = ageGroups
            };

            // Act
            await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.That(capturedRequest, Is.Not.Null);
            Assert.That(capturedRequest!.ProcessStatusFilter, Is.EqualTo(processStatuses));
            Assert.That(capturedRequest.AgeGroups, Is.EqualTo(ageGroups));
        }
    }
}
