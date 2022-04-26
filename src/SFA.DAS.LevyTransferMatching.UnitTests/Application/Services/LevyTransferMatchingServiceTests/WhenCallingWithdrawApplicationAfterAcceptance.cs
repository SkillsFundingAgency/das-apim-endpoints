using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Services;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Services.LevyTransferMatchingServiceTests
{
    public class WhenCallingWithdrawApplicationAfterAcceptance
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Result(
            WithdrawApplicationAfterAcceptanceRequest request,
            ApiResponse<WithdrawApplicationAfterAcceptanceRequest> response,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockLevyTransferMatchingApiClient,
            LevyTransferMatchingService levyTransferMatchingService)
        {
            mockLevyTransferMatchingApiClient
                .Setup(x => x.PostWithResponseCode<WithdrawApplicationAfterAcceptanceRequest>(It.Is<WithdrawApplicationAfterAcceptanceRequest>(y => y.AccountId == request.AccountId &&
                                                                                                                                                    y.ApplicationId == request.ApplicationId &&
                                                                                                                                                    y.PostUrl == request.PostUrl)))
                                                                                                                                                .ReturnsAsync(response);

            var result = await levyTransferMatchingService.WithdrawApplicationAfterAcceptance(request);

            Assert.AreEqual(result.StatusCode, response.StatusCode);
            Assert.AreEqual(result.ErrorContent, response.ErrorContent);
        }
    }
}
