using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Requests;
using SFA.DAS.ProviderFeedback.Application.InnerApi.Responses;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedbackAnnual;
using SFA.DAS.ProviderFeedback.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.SharedOuterApi.InnerApi.Responses.TrainingProviderService.TrainingProviderResponse;

namespace SFA.DAS.ProviderFeedback.UnitTests.Application.GetProviderFeedback
{
    public class WhenGettingProviderFeedbackAnnual
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_With_The_Request_And_The_ProviderFeedback_Is_Returned(
            GetProviderFeedbackAnnualQuery query,
            GetEmployerFeedbackAnnualResponse employerApiResponse,
            GetApprenticeFeedbackAnnualResponse apprenticeApiResponse,
            TrainingProviderResponse providerStandardsData,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> apprenticeFeedbackApiClient,
            [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> employerFeedbackApiClient,
            [Frozen] Mock<IProviderService> providerService,
            GetProviderFeedbackAnnualQueryHandler handler)
        {
            providerStandardsData.ProviderType.Id = (short)ProviderTypeIdentifier.EmployerProvider;
            providerService.Setup(x => x.GetTrainingProviderDetails(It.IsAny<long>())).ReturnsAsync(providerStandardsData);

            apprenticeFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeFeedbackAnnualResponse>(It.IsAny<GetApprenticeFeedbackAnnualRequest>()))
                .ReturnsAsync(new ApiResponse<GetApprenticeFeedbackAnnualResponse>(apprenticeApiResponse, HttpStatusCode.OK, string.Empty));

            employerFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetEmployerFeedbackAnnualResponse>(It.IsAny<GetEmployerFeedbackAnnualRequest>()))
                .ReturnsAsync(new ApiResponse<GetEmployerFeedbackAnnualResponse>(employerApiResponse, HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.ProviderStandard.Should().NotBeNull();
            actual.ProviderStandard.Ukprn.Should().Be(query.ProviderId);
            actual.ProviderStandard.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeApiResponse);
            actual.ProviderStandard.IsEmployerProvider.Should().BeTrue();

            apprenticeFeedbackApiClient.Verify(x => x.GetWithResponseCode<GetApprenticeFeedbackAnnualResponse>(It.IsAny<GetApprenticeFeedbackAnnualRequest>()), Times.Once);

            employerFeedbackApiClient.Verify(x => x.GetWithResponseCode<GetEmployerFeedbackAnnualResponse>(It.IsAny<GetEmployerFeedbackAnnualRequest>()), Times.Never);
        }

        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_For_Both_Apprentice_And_Employer_Feedback_When_Not_EmployerProvider(
            GetProviderFeedbackAnnualQuery query,
            GetEmployerFeedbackAnnualResponse employerApiResponse,
            GetApprenticeFeedbackAnnualResponse apprenticeApiResponse,
            TrainingProviderResponse providerStandardsData,
            [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> apprenticeFeedbackApiClient,
            [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> employerFeedbackApiClient,
            [Frozen] Mock<IProviderService> providerService,
            GetProviderFeedbackAnnualQueryHandler handler)
        {
            providerStandardsData.ProviderType.Id = (short)ProviderTypeIdentifier.MainProvider;
            providerService.Setup(x => x.GetTrainingProviderDetails(It.IsAny<long>())).ReturnsAsync(providerStandardsData);

            apprenticeFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetApprenticeFeedbackAnnualResponse>(It.IsAny<GetApprenticeFeedbackAnnualRequest>()))
                .ReturnsAsync(new ApiResponse<GetApprenticeFeedbackAnnualResponse>(apprenticeApiResponse, HttpStatusCode.OK, string.Empty));

            employerFeedbackApiClient
                .Setup(x => x.GetWithResponseCode<GetEmployerFeedbackAnnualResponse>(It.IsAny<GetEmployerFeedbackAnnualRequest>()))
                .ReturnsAsync(new ApiResponse<GetEmployerFeedbackAnnualResponse>(employerApiResponse, HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Should().NotBeNull();
            actual.ProviderStandard.Should().NotBeNull();
            actual.ProviderStandard.Ukprn.Should().Be(query.ProviderId);
            actual.ProviderStandard.ApprenticeFeedback.Should().BeEquivalentTo(apprenticeApiResponse);
            actual.ProviderStandard.EmployerFeedback.Should().BeEquivalentTo(employerApiResponse);
            actual.ProviderStandard.IsEmployerProvider.Should().BeFalse();

            apprenticeFeedbackApiClient.Verify(x => x.GetWithResponseCode<GetApprenticeFeedbackAnnualResponse>(It.IsAny<GetApprenticeFeedbackAnnualRequest>()), Times.Once);

            employerFeedbackApiClient.Verify(x => x.GetWithResponseCode<GetEmployerFeedbackAnnualResponse>(It.IsAny<GetEmployerFeedbackAnnualRequest>()), Times.Once);
        }
    }
}
