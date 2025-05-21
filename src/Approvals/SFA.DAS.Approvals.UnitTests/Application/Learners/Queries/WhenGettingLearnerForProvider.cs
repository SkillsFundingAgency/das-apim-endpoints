using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application.Learners.Queries;
using SFA.DAS.Approvals.InnerApi.LearnerData;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Approvals.UnitTests.Application.Learners.Queries;

public class WhenGettingLearnerForProvider
{
    [Test, MoqAutoData]
    public async Task Then_The_Api_Is_Called_And_The_result_Is_Returned(
        GetLearnerForProviderQuery query,
        GetLearnerForProviderRequest learnerRequest,
        GetLearnerForProviderResponse learnerResponse,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> learnerDataClient,
        [Greedy] GetLearnerForProviderQueryHandler handler
    )
    {
        GetLearnersForProviderRequest input;
        learnerDataClient.Setup(x =>
                x.Get<GetLearnerForProviderResponse>(It.IsAny<GetLearnerForProviderRequest>()))
            .ReturnsAsync(learnerResponse);

        var actual = await handler.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(learnerResponse);
    }
}