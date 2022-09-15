using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetFeedbackTargetsForUpdate;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Apprentices.Queries
{    
    public class WhenHandlingGetApprenticeFeedbackTargetsForUpdate
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_ApprenticeFeedbackTargets_From_The_Api(
           List<ApprenticeFeedbackTarget> feedbackTargets,
           GetFeedbackTargetsForUpdateQuery query,
           [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> mockApprenticeFeedbackApiClient,
           GetFeedbackTargetsForUpdateQueryHandler handler)
        {
            mockApprenticeFeedbackApiClient
                .Setup(client => client.GetAll<ApprenticeFeedbackTarget>(
                It.IsAny<GetApprenticeFeedbackTargetsForUpdateRequest>()))
                .ReturnsAsync(feedbackTargets);

            var actual = await handler.Handle(query, CancellationToken.None);
            actual.FeedbackTargetsForUpdate.Should().BeEquivalentTo(feedbackTargets, options => options.ExcludingMissingMembers());
        }
    }
}
