using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Services;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Services.LevyTransferMatchingServiceTests
{
    [TestFixture]
    public class WhenCallingGetAccount
    {
        public Fixture Fixture = new Fixture();

        [Test]
        public async Task Then_The_Api_Is_Called()
        {
            var getAccountRequest = Fixture.Create<GetAccountRequest>();

            var apiClient = new Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>>();
            apiClient.Setup(x => x.Get<GetAccountResponse>(It.IsAny<GetAccountRequest>())).ReturnsAsync(() => new GetAccountResponse());

            var service = new LevyTransferMatchingService(apiClient.Object);

            await service.GetAccount(getAccountRequest);

            apiClient.Verify(x => x.Get<GetAccountResponse>(It.Is<GetAccountRequest>(r =>
                r.GetUrl.Equals($"/accounts/{getAccountRequest.AccountId}"))));
        }
    }
}
