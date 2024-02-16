using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Applications.GetDeclined;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Applications.GetDeclined
{
    public class WhenCallingHandle
    {
        private Fixture _fixture;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
        }

        [Test, MoqAutoData]
        public async Task And_Application_Exists_Response_Returned(
            GetDeclinedQuery getDeclinedQuery,
            GetApplicationResponse getApplicationResponse,
            Models.Pledge pledgeResponse,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetDeclinedQueryHandler getDeclinedQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getDeclinedQuery.ApplicationId.ToString()))))
                .ReturnsAsync(getApplicationResponse);
           
            mockLevyTransferMatchingService
                .Setup(x => x.GetPledge(It.Is<int>(y => y == getApplicationResponse.PledgeId)))
                .ReturnsAsync(pledgeResponse);

            var result = await getDeclinedQueryHandler.Handle(getDeclinedQuery, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.AreEqual(pledgeResponse.DasAccountName, result.EmployerAccountName);
        }

        [Test, MoqAutoData]
        public async Task And_Application_Doesnt_Exist_Returns_Null(
            GetDeclinedQuery getDeclinedQuery,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetDeclinedQueryHandler getDeclinedQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getDeclinedQuery.ApplicationId.ToString()))))
                .ReturnsAsync((GetApplicationResponse)null);

            var result = await getDeclinedQueryHandler.Handle(getDeclinedQuery, CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}