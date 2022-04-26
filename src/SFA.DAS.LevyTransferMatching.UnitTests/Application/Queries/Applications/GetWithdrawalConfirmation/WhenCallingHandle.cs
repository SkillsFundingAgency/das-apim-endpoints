using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetWithdrawalConfirmation;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Applications.GetWithdrawalConfirmation
{
    [TestFixture]
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_Application_Exists_Response_Returned(
            GetWithdrawalConfirmationQuery getWithdrawalConfirmationQuery,
            GetApplicationResponse getApplicationResponse,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetWithdrawalConfirmationQueryHandler getWithdrawalConfirmationQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getWithdrawalConfirmationQuery.ApplicationId.ToString()))))
                .ReturnsAsync(getApplicationResponse);

            var result = await getWithdrawalConfirmationQueryHandler.Handle(getWithdrawalConfirmationQuery, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(getApplicationResponse.SenderEmployerAccountName, result.PledgeEmployerName);
        }

        [Test, MoqAutoData]
        public async Task And_Application_Doesnt_Exist_Returns_Null(
            GetWithdrawalConfirmationQuery getWithdrawalConfirmationQuery,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetWithdrawalConfirmationQueryHandler getWithdrawalConfirmationQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getWithdrawalConfirmationQuery.ApplicationId.ToString()))))
                .ReturnsAsync((GetApplicationResponse)null);

            var result = await getWithdrawalConfirmationQueryHandler.Handle(getWithdrawalConfirmationQuery, CancellationToken.None);

            Assert.IsNull(result);
        }
    }
}
