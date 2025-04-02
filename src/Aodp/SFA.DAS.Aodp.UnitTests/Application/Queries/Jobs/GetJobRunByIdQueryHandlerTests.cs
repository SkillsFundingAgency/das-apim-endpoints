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
    public class GetJobRunByIdQueryHandlerTests
    {
        private readonly IFixture _fixture;
        private Mock<IAodpApiClient<AodpApiConfiguration>> _apiClientMock;
        private readonly GetJobRunByIdQueryHandler _handler;

        public GetJobRunByIdQueryHandlerTests()
        {
            _fixture = new Fixture().Customize(new AutoMoqCustomization());
            _apiClientMock = _fixture.Freeze<Mock<IAodpApiClient<AodpApiConfiguration>>>();
            _handler = _fixture.Create<GetJobRunByIdQueryHandler>();
        }

        [Test]
        public async Task Then_JobRun_Is_Returned()
        {
            // Arrange
            var query = _fixture.Create<GetJobRunByIdQuery>();           
            var body = _fixture.Build<GetJobRunByIdApiResponse>()
                                .Create();
            var apiResponse = new ApiResponse<GetJobRunByIdApiResponse>(body, System.Net.HttpStatusCode.OK, "");

            _apiClientMock.Setup(x => x.GetWithResponseCode<GetJobRunByIdApiResponse>(It.IsAny<GetJobRunByIdApiRequest>()))
                          .ReturnsAsync(apiResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            _apiClientMock.Verify(x => x.GetWithResponseCode<GetJobRunByIdApiResponse>(It.IsAny<GetJobRunByIdApiRequest>()), Times.Once);
            Assert.That(result.Success, Is.True);
            Assert.That(apiResponse.Body.Id, Is.EqualTo(result.Value.Id));
        }
    }
}


