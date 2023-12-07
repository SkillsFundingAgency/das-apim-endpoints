using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProviders;
using SFA.DAS.ApprenticeFeedback.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.UnitTests.Application.Queries
{
    public class WhenHandlingGetApprenticeTrainingProviders
    {
        [Test, MoqAutoData]
        public async Task Then_TheApprenticeFeedbackApiIsCalledWithTheRequest_And_ReturnsTrainingProviders(
                GetApprenticeTrainingProvidersQuery query,
                [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> feedbackApiClient,
                GetApprenticeTrainingProvidersQueryHandler handler,
                GetApprenticeTrainingProvidersResult apiResponse)
        {
            feedbackApiClient.Setup(x => x.Get<GetApprenticeTrainingProvidersResult>(
                    It.Is<GetAllTrainingProvidersForApprenticeRequest>(x => x.ApprenticeId == query.ApprenticeId))).ReturnsAsync(apiResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}
