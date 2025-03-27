using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;
using SFA.DAS.Aodp.Application.Queries.Jobs;
using SFA.DAS.Aodp.InnerApi.AodpApi.Jobs;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.AODP.Tests.Application.Queries
{
    [TestFixture]
    public class GetJobByNameQueryHandlerTests
    {
        private readonly IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private readonly GetJobByNameQueryHandler _handler;

        public GetJobByNameQueryHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetJobByNameQueryHandler>();
        }

        [Test]
        public async Task Then_JobData_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetJobByNameQuery>();                      
            var body = _fixture.Build<GetJobByNameApiResponse>()
                                .Create();
            var apiResponse = new ApiResponse<GetJobByNameApiResponse>(body, System.Net.HttpStatusCode.OK, "");

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetJobByNameApiResponse>(It.IsAny<GetJobByNameApiRequest>()))
                          .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetJobByNameApiResponse>(It.IsAny<GetJobByNameApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.True);
            Assert.That(apiResponse.Body.Id, Is.EqualTo(result.Value.Id));
        }        
    }
}


