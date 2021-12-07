using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.LevyTransferMatching.Queries;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.LevyTransferMatching.Queries
{
    public class WhenGettingAPledgeApplication
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_Application_Is_Returned(
            GetPledgeApplicationQuery query,
            GetPledgeApplicationResponse apiResponse,
            [Frozen] Mock<ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration>> apiClient,
            GetPledgeApplicationQueryHandler handler
            )
        {
            apiClient.Setup(x => x.Get<GetPledgeApplicationResponse>(It.Is<GetPledgeApplicationRequest>(x=>x.PledgeApplicationId == query.PledgeApplicationId))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}