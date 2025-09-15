using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ApprenticeFeedback;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerFeedback;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Services;
public class CachedFeedbackServiceTests
{
    [Test, MoqAutoData]
    public async Task GetProviderFeedback_NoCache_InvokesEmployerFeedbackApi(
        [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> empFBApiClientMock,
        [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
        EmployerFeedbackAnnualDetails expected,
        CachedFeedbackService sut,
        int ukprn)
    {
        // Arrange
        cacheStorageServiceMock
            .Setup(x => x.RetrieveFromCache<EmployerFeedbackAnnualDetails>($"{nameof(EmployerFeedbackAnnualDetails)}-{ukprn}"))
            .ReturnsAsync((EmployerFeedbackAnnualDetails)null);
        empFBApiClientMock
            .Setup(e => e.GetWithResponseCode<EmployerFeedbackAnnualDetails>(It.Is<GetEmployerFeedbackSummaryAnnualRequest>(r => r.GetUrl.Contains(ukprn.ToString()))))
            .ReturnsAsync(new ApiResponse<EmployerFeedbackAnnualDetails>(expected, System.Net.HttpStatusCode.OK, null));
        // Act
        var (actual, _) = await sut.GetProviderFeedback(ukprn);
        // Assert
        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task GetProviderFeedback_NoCache_InvokesApprenticeFeedbackApi(
        [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> appFBApiClientMock,
        [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
        ApprenticeFeedbackAnnualDetails expected,
        CachedFeedbackService sut,
        int ukprn)
    {
        // Arrange
        cacheStorageServiceMock
            .Setup(x => x.RetrieveFromCache<ApprenticeFeedbackAnnualDetails>($"{nameof(ApprenticeFeedbackAnnualDetails)}-{ukprn}"))
            .ReturnsAsync((ApprenticeFeedbackAnnualDetails)null);
        appFBApiClientMock
            .Setup(e => e.GetWithResponseCode<ApprenticeFeedbackAnnualDetails>(It.Is<GetApprenticeFeedbackSummaryAnnualRequest>(r => r.GetUrl.Contains(ukprn.ToString()))))
            .ReturnsAsync(new ApiResponse<ApprenticeFeedbackAnnualDetails>(expected, System.Net.HttpStatusCode.OK, null));
        // Act
        var (_, actual) = await sut.GetProviderFeedback(ukprn);
        // Assert
        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task GetProviderFeedback_CacheHit_ReturnsCachedEmployerFeedback(
        [Frozen] Mock<IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration>> empFBApiClientMock,
        [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
        EmployerFeedbackAnnualDetails expected,
        CachedFeedbackService sut,
        int ukprn)
    {
        // Arrange
        cacheStorageServiceMock
            .Setup(x => x.RetrieveFromCache<EmployerFeedbackAnnualDetails>($"{nameof(EmployerFeedbackAnnualDetails)}-{ukprn}"))
            .ReturnsAsync(expected);
        // Act
        var (actual, _) = await sut.GetProviderFeedback(ukprn);
        // Assert
        actual.Should().Be(expected);
        empFBApiClientMock.Verify(e => e.GetWithResponseCode<EmployerFeedbackAnnualDetails>(It.IsAny<GetEmployerFeedbackSummaryAnnualRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task GetProviderFeedback_CacheHit_ReturnsCachedApprenticeFeedback(
        [Frozen] Mock<IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration>> appFBApiClientMock,
        [Frozen] Mock<ICacheStorageService> cacheStorageServiceMock,
        ApprenticeFeedbackAnnualDetails expected,
        CachedFeedbackService sut,
        int ukprn)
    {
        // Arrange
        cacheStorageServiceMock
            .Setup(x => x.RetrieveFromCache<ApprenticeFeedbackAnnualDetails>($"{nameof(ApprenticeFeedbackAnnualDetails)}-{ukprn}"))
            .ReturnsAsync(expected);
        // Act
        var (_, actual) = await sut.GetProviderFeedback(ukprn);
        // Assert
        actual.Should().Be(expected);
        appFBApiClientMock.Verify(e => e.GetWithResponseCode<ApprenticeFeedbackAnnualDetails>(It.IsAny<GetApprenticeFeedbackSummaryAnnualRequest>()), Times.Never);
    }
}
