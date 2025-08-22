using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetProvider
{
    [TestFixture]
    public class GetProviderQueryHandlerTest
    {
        private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _commitmentsV2ApiClientMock;
        private GetProviderQueryHandler _handler;

        [SetUp]
        public void SetUp()
        {
            _commitmentsV2ApiClientMock = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();
            _handler = new GetProviderQueryHandler(_commitmentsV2ApiClientMock.Object);
        }

        [Test]
        public async Task Handle_ShouldReturnProvider_WhenProviderExists()
        {
            var providerId = 12345678;
            var query = new GetProviderQuery { ProviderId = providerId };
            var expectedProvider = new GetProviderQueryResult
            {
                Name = "Test Provider",
                ProviderId = providerId
            };

            _commitmentsV2ApiClientMock
                .Setup(x => x.Get<GetProviderQueryResult>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync(expectedProvider);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.That(result != null);
            Assert.That(expectedProvider.Name, Is.EqualTo(result.Name));
            Assert.That(expectedProvider.ProviderId, Is.EqualTo(result.ProviderId));
        }

        [Test]
        public Task Handle_ThrowsException_WhenApiCallFails()
        {
            var providerId = 12345678;
            var query = new GetProviderQuery { ProviderId = providerId };
            _commitmentsV2ApiClientMock
                .Setup(x => x.Get<GetProviderQueryResult>(It.IsAny<GetProviderRequest>()))
                .ThrowsAsync(new System.Exception("API call failed"));

            Assert.ThrowsAsync<System.Exception>(async () =>
            {
                await _handler.Handle(query, CancellationToken.None);
            });
            return Task.CompletedTask;
        }

        [Test]
        public async Task Handle_ShouldReturnNull_WhenProviderDoesNotExist()
        {
            var providerId = 12345678;
            var query = new GetProviderQuery { ProviderId = providerId };
            _commitmentsV2ApiClientMock
                .Setup(x => x.Get<GetProviderQueryResult>(It.IsAny<GetProviderRequest>()))
                .ReturnsAsync((GetProviderQueryResult)null);

            var result = await _handler.Handle(query, CancellationToken.None);

            Assert.That(result == null);
        }
        
        [Test]
        public void GetUrl_ShouldReturnCorrectUrl_WithGivenProviderId()
        {
            var providerId = 12345;
            var request = new GetProviderRequest(providerId);
            var url = request.GetUrl;

            Assert.That($"api/providers/{providerId}", Is.EqualTo(url));
        }
    }
}