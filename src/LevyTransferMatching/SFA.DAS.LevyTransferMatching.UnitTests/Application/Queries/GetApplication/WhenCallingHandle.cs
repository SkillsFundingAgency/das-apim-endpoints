using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetApplication
{
    public class WhenCallingHandle
    {
        private readonly Fixture _fixture;

        public WhenCallingHandle()
        {
            _fixture = new Fixture();
        }

        [Test, MoqAutoData]
        public async Task And_Application_Exists_Stitches_Up_Standard_And_Location_To_Result(
            GetApplicationQuery getApplicationQuery,
            GetApplicationResponse getApplicationResponse,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetApplicationQueryHandler getApplicationQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getApplicationQuery.PledgeId.ToString()) && y.GetUrl.Contains(getApplicationQuery.ApplicationId.ToString()))))
                .ReturnsAsync(getApplicationResponse);

            var result = await getApplicationQueryHandler.Handle(getApplicationQuery, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AboutOpportunity, Is.EqualTo(getApplicationResponse.Details));
        }

        [Test, MoqAutoData]
        public async Task And_Application_Exists_And_No_Specific_Locations_Returned_Stitches_Up_Standard_And_Location_To_Result(
            GetApplicationQuery getApplicationQuery,
            GetApplicationResponse getApplicationResponse,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetApplicationQueryHandler getApplicationQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getApplicationQuery.PledgeId.ToString()) && y.GetUrl.Contains(getApplicationQuery.ApplicationId.ToString()))))
                .ReturnsAsync(getApplicationResponse);

            var result = await getApplicationQueryHandler.Handle(getApplicationQuery, CancellationToken.None);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.AboutOpportunity, Is.EqualTo(getApplicationResponse.Details));
        }

        [Test, MoqAutoData]
        public async Task And_Application_Doesnt_Exist_Returns_Null(
            GetApplicationQuery getApplicationQuery,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            GetApplicationQueryHandler getApplicationQueryHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(getApplicationQuery.PledgeId.ToString()) && y.GetUrl.Contains(getApplicationQuery.ApplicationId.ToString()))))
                .ReturnsAsync((GetApplicationResponse)null);

            var result = await getApplicationQueryHandler.Handle(getApplicationQuery, CancellationToken.None);

            Assert.That(result, Is.Null);
        }
    }
}