using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackForAcademicYear;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderFeedback.UnitTests.Application.GetProviderFeedback
{
    public class WhenGettingProviderFeedbackForAcademicYear
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_ProviderFeedback_Is_Returned(
            GetProviderFeedbackForAcademicYearQuery query,
            GetEmployerFeedbackForAcademicYearResponse employerapiResponse,
            GetApprenticeFeedbackForAcademicYearResponse apprenticeapiResponse,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> apprenticeFeedbackApiClient,
            [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> _employerFeedbackApiClient,
            GetProviderFeedbackForAcademicYearQueryHandler handler
        )
        {
            apprenticeFeedbackApiClient.Setup(x => x.GetWithResponseCode<GetApprenticeFeedbackForAcademicYearResponse>(It.IsAny<GetApprenticeFeedbackForAcademicYearRequest>())).ReturnsAsync(new ApiResponse<GetApprenticeFeedbackForAcademicYearResponse>(apprenticeapiResponse, HttpStatusCode.OK, string.Empty));
            _employerFeedbackApiClient.Setup(x => x.GetWithResponseCode<GetEmployerFeedbackForAcademicYearResponse>(It.IsAny<GetEmployerFeedbackForAcademicYearRequest>())).ReturnsAsync(new ApiResponse<GetEmployerFeedbackForAcademicYearResponse>(employerapiResponse, HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.ProviderStandard.Should().NotBeNull();
            actual.ProviderStandard.Ukprn.Should().Be(query.ProviderId);
            actual.ProviderStandard.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeapiResponse);
            actual.ProviderStandard.EmployerFeedback.Should().BeEquivalentTo(employerapiResponse);
        }
    }
}