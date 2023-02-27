using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Services;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Services.LevyTransferMatchingServiceTests
{
    [TestFixture]
    public class WhenCallingCreateAccount
    {
        public Fixture Fixture = new Fixture();
        
        [Test]
        public async Task Then_The_Api_Is_Called()
        {
            var createAccountRequest = Fixture.Create<CreateAccountRequest>();

            var apiClient = new Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>>();
            apiClient
                .Setup(x => x.PostWithResponseCode<CreateAccountRequest>(It.IsAny<CreateAccountRequest>(), true))
                .ReturnsAsync(new ApiResponse<CreateAccountRequest>(null, HttpStatusCode.Created, ""));

            var service = new LevyTransferMatchingService(apiClient.Object);

            await service.CreateAccount(createAccountRequest);

            apiClient.Verify(x => x.PostWithResponseCode<CreateAccountRequest>(It.Is<CreateAccountRequest>(r =>
                r.PostUrl.StartsWith("/accounts")
                && r.Data == createAccountRequest.Data
                ), false));
        }
    }
}