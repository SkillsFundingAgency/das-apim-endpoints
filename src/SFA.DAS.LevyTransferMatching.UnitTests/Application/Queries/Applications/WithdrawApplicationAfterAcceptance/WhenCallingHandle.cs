using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.WithdrawApplicationAfterAcceptance;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Applications.WithdrawApplicationAfterAcceptance
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_Response_Returned(
            WithdrawApplicationAfterAcceptanceCommand command,
            ApiResponse<WithdrawApplicationAfterAcceptanceRequest> response,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            WithdrawApplicationAfterAcceptanceCommandHandler handler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.WithdrawApplicationAfterAcceptance(It.Is<WithdrawApplicationAfterAcceptanceRequest>(y => y.PostUrl.Contains(command.AccountId.ToString()) &&
                                                                                                                       y.PostUrl.Contains(command.ApplicationId.ToString())), It.IsAny<CancellationToken>()))
                .ReturnsAsync(response);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(response.StatusCode, result.StatusCode);
            Assert.AreEqual(response.ErrorContent, result.ErrorContent);
        }
    }
}
