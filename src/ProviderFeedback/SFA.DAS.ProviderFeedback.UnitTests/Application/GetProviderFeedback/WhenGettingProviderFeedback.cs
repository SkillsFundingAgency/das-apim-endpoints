using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderFeedback.UnitTests.Application.GetProviderFeedback
{
    public class WhenGettingProviderFeedback
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_ProviderFeedback_Is_Returned(
            GetProviderFeedbackQuery query,
            GetEmployerFeedbackResponse employerapiResponse,
            GetApprenticeFeedbackResponse apprenticeapiResponse,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> apprenticeFeedbackApiClient,
            [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _employerFeedbackApiClient,
            GetProviderFeedbackQueryHandler handler
        )
        {
            apprenticeFeedbackApiClient.Setup(x => x.GetWithResponseCode<GetApprenticeFeedbackResponse>(It.IsAny<GetApprenticeFeedbackRequest>())).ReturnsAsync(new ApiResponse<GetApprenticeFeedbackResponse>(apprenticeapiResponse, HttpStatusCode.OK, string.Empty));
            _employerFeedbackApiClient.Setup(x => x.GetWithResponseCode<GetEmployerFeedbackResponse>(It.IsAny<GetEmployerFeedbackRequest>())).ReturnsAsync(new ApiResponse<GetEmployerFeedbackResponse>(employerapiResponse, HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.ProviderStandard.Should().NotBeNull();
            actual.ProviderStandard.Ukprn.Should().Be(query.ProviderId);
            actual.ProviderStandard.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeapiResponse);
            actual.ProviderStandard.EmployerFeedback.Should().BeEquivalentTo(employerapiResponse);
        }
    }
}