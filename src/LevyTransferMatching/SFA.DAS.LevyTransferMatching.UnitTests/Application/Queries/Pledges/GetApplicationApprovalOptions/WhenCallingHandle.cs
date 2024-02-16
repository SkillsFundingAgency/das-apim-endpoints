using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetApplicationApprovalOptions;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetApplicationApprovalOptions
{
    [TestFixture]
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_Application_Exists_Response_Returned(
            GetApplicationApprovalOptionsQuery getApplicationApprovalOptionsQuery,
            GetApplicationResponse getApplicationResponse,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetApplicationApprovalOptionsQueryHandler getApplicationApprovalOptionsQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getApplicationApprovalOptionsQuery.ApplicationId.ToString()) && y.GetUrl.Contains(getApplicationApprovalOptionsQuery.PledgeId.ToString()))))
                .ReturnsAsync(getApplicationResponse);

            var result = await getApplicationApprovalOptionsQueryHandler.Handle(getApplicationApprovalOptionsQuery, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(getApplicationResponse.EmployerAccountName, result.EmployerAccountName);
            Assert.AreEqual(getApplicationResponse.Status, result.ApplicationStatus);
        }

        [Test, MoqAutoData]
        public async Task And_Application_Doesnt_Exist_Returns_Null(
            GetApplicationApprovalOptionsQuery getApplicationApprovalOptionsQuery,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetApplicationApprovalOptionsQueryHandler getApplicationApprovalOptionsQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getApplicationApprovalOptionsQuery.ApplicationId.ToString()))))
                .ReturnsAsync((GetApplicationResponse)null);

            var result = await getApplicationApprovalOptionsQueryHandler.Handle(getApplicationApprovalOptionsQuery, CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}
