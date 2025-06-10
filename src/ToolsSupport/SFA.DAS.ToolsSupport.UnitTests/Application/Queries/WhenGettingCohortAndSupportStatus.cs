using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using FluentAssertions;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;

public class WhenGettingCohortAndSupportStatus
{
    [Test, MoqAutoData]
    public async Task Then_Gets_CohortDetails_And_SupportStatus(
        long cohortId,
        GetCohortByIdResponse mockApiResponse,
        GetCohortSupportStatusByIdResponse mockApiStatusResponse,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetCohortAndSupportStatusQueryHandler sut)
    {
        var mockCohortQuery = new GetCohortAndSupportStatusQuery { CohortId = cohortId };
        var expectedUrl = $"api/cohorts/{cohortId}";
        mockApiClient.Setup(client => client.Get<GetCohortByIdResponse>(It.Is<GetCohortByIdRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync(mockApiResponse);

        var expectedStatusUrl = $"api/cohorts/{cohortId}/support-status";
        mockApiClient.Setup(client => client.Get<GetCohortSupportStatusByIdResponse>(It.Is<GetCohortSupportStatusByIdRequest>(c => c.GetUrl == expectedStatusUrl)))
            .ReturnsAsync(mockApiStatusResponse);

        var actual = await sut.Handle(mockCohortQuery, It.IsAny<CancellationToken>());

        actual.CohortId.Should().Be(mockApiResponse.CohortId);
        actual.CohortReference.Should().Be(mockApiResponse.CohortReference);
        actual.EmployerAccountId.Should().Be(mockApiResponse.AccountId);
        actual.EmployerAccountName.Should().Be(mockApiResponse.LegalEntityName);
        actual.ProviderName.Should().Be(mockApiResponse.ProviderName);
        actual.UkPrn.Should().Be(mockApiResponse.ProviderId);
        actual.NoOfApprentices.Should().Be(mockApiStatusResponse.NoOfApprentices);
        actual.CohortStatus.Should().Be(mockApiStatusResponse.CohortStatus);
    }

    [Test, MoqAutoData]
    public async Task Then_Returns_Null_When_No_Matching_Cohort(
        long cohortId,
        GetCohortByIdResponse mockApiResponse,
        GetCohortSupportStatusByIdResponse mockApiStatusResponse,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetCohortAndSupportStatusQueryHandler sut)
    {
        var mockCohortQuery = new GetCohortAndSupportStatusQuery { CohortId = cohortId };
        var expectedUrl = $"api/cohorts/{cohortId}";
        mockApiClient.Setup(client => client.Get<GetCohortByIdResponse?>(It.Is<GetCohortByIdRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync((GetCohortByIdResponse?)null);

        var expectedStatusUrl = $"api/cohorts/{cohortId}/support-status";
        mockApiClient.Setup(client => client.Get<GetCohortSupportStatusByIdResponse?>(It.Is<GetCohortSupportStatusByIdRequest>(c => c.GetUrl == expectedStatusUrl)))
            .ReturnsAsync((GetCohortSupportStatusByIdResponse?)null);

        var actual = await sut.Handle(mockCohortQuery, It.IsAny<CancellationToken>());

        actual.Should().BeNull();
    }
}