using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Services;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Pledges;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Services.LevyTransferMatchingServiceTests
{
    public class WhenCallingCreatePledge
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_PledgeReference(
            Pledge pledge,
            PledgeReference pledgeReference,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> mockLevyTransferMatchingApiClient,
            LevyTransferMatchingService levyTransferMatchingService)
        {
            mockLevyTransferMatchingApiClient
                .Setup(x => x.PostWithResponseCode<PledgeReference>(It.Is<CreatePledgeRequest>(y => y.PostUrl.Contains(pledge.EncodedAccountId))))
                .ReturnsAsync(() => new ApiResponse<PledgeReference>(pledgeReference, HttpStatusCode.Accepted, null));

            var actual = await levyTransferMatchingService.CreatePledge(pledge);

            Assert.AreEqual(actual, pledgeReference);
        }
    }
}