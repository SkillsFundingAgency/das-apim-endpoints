using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using FluentAssertions;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.ToolsSupport.Mappers;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;

public class WhenGettingApprenticeshipDetails
{
    [Test, MoqAutoData]
    public async Task Then_Gets_ApprenticeshipDetails_And_Returns_Core_Values_Correctly(
        long id,
        SupportApprenticeshipDetails mockApiApprenticeshipDetailsResponse,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        var mockQuery = new GetApprenticeshipQuery {Id = id};
        var expectedApprenticeDetailsUrl = $"api/apprenticeships/{id}/approved-apprenticeship";
        mockApiClient.Setup(client => client.Get<SupportApprenticeshipDetails>(
                It.Is<GetApprovedApprenticeshipByIdRequest>(c =>
                    c.GetUrl == expectedApprenticeDetailsUrl)))
            .ReturnsAsync(mockApiApprenticeshipDetailsResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Should().BeEquivalentTo(
                new
                {
                    ApprenticeshipId = mockApiApprenticeshipDetailsResponse.Id,
                    EmployerAccountId = mockApiApprenticeshipDetailsResponse.EmployerAccountId,
                    AgreementStatus = mockApiApprenticeshipDetailsResponse.AgreementStatus.GetDescription(),
                    MadeRedundant = mockApiApprenticeshipDetailsResponse.MadeRedundant,
                    Uln = mockApiApprenticeshipDetailsResponse.Uln,
                    Email = mockApiApprenticeshipDetailsResponse.Email,
                    TrainingProvider = mockApiApprenticeshipDetailsResponse.ProviderName,
                    FirstName = mockApiApprenticeshipDetailsResponse.FirstName,
                    LastName = mockApiApprenticeshipDetailsResponse.LastName,
                    DateOfBirth = mockApiApprenticeshipDetailsResponse.DateOfBirth,
                    CohortReference = mockApiApprenticeshipDetailsResponse.CohortReference,
                    EmployerReference = mockApiApprenticeshipDetailsResponse.EmployerRef,
                    LegalEntity = mockApiApprenticeshipDetailsResponse.EmployerName,
                    UKPRN = mockApiApprenticeshipDetailsResponse.ProviderId,
                    Trainingcourse = mockApiApprenticeshipDetailsResponse.CourseName,
                    Option = mockApiApprenticeshipDetailsResponse.TrainingCourseOption,
                    ApprenticeshipCode = mockApiApprenticeshipDetailsResponse.CourseCode,
                    TrainingStartDate = mockApiApprenticeshipDetailsResponse.StartDate,
                    TrainingEndDate = mockApiApprenticeshipDetailsResponse.EndDate,
                });
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_ApprenticeshipDetails_And_Returns_Calculated_Values_Correctly(
        long id,
        SupportApprenticeshipDetails mockApiApprenticeshipDetailsResponse,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        var mockQuery = new GetApprenticeshipQuery { Id = id };
        mockApiClient.Setup(client => client.Get<SupportApprenticeshipDetails>(
                It.IsAny<GetApprovedApprenticeshipByIdRequest>()))
            .ReturnsAsync(mockApiApprenticeshipDetailsResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Should().BeEquivalentTo(
                new
                {
                    PauseDate = mockApiApprenticeshipDetailsResponse.PaymentStatus == PaymentStatus.Paused ? mockApiApprenticeshipDetailsResponse.PauseDate : (DateTime?) null,
                    CompletionDate = mockApiApprenticeshipDetailsResponse.PaymentStatus == PaymentStatus.Completed ? mockApiApprenticeshipDetailsResponse.CompletionDate : null,
                    StopDate = mockApiApprenticeshipDetailsResponse.PaymentStatus == PaymentStatus.Withdrawn ? mockApiApprenticeshipDetailsResponse.StopDate : null,
                    Version = mockApiApprenticeshipDetailsResponse.TrainingCourseVersionConfirmed ? mockApiApprenticeshipDetailsResponse.TrainingCourseVersion : null,
                    ConfirmationStatusDescription = mockApiApprenticeshipDetailsResponse.ConfirmationStatus == null ? mockApiApprenticeshipDetailsResponse.ConfirmationStatus.ToString() : null,
                });
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_ApprenticeshipDetails_And_Returns_PendingChnages_Correctly(
        long id,
        PendingChangesResponse pendingChanges,
        SupportApprenticeshipDetails mockApiApprenticeshipDetailsResponse,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        [Frozen] Mock<IPendingChangesMapper> mockPendingChanges,
        GetApprenticeshipQueryHandler sut)
    {
        var mockQuery = new GetApprenticeshipQuery { Id = id };
        mockApiClient.Setup(client => client.Get<SupportApprenticeshipDetails>(
                It.IsAny<GetApprovedApprenticeshipByIdRequest>()))
            .ReturnsAsync(mockApiApprenticeshipDetailsResponse);

        mockPendingChanges.Setup(x => x.CreatePendingChangesResponse(It.IsAny<GetApprenticeshipPendingUpdatesResponse>(),
            It.IsAny<SupportApprenticeshipDetails>())).Returns(pendingChanges);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.PendingChanges.Should().BeEquivalentTo(pendingChanges);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_ApprenticeshipDetails_And_Returns_ChangeOfProvidChain_Correctly(
        long id,
        GetApprenticeshipChangeOfProviderChainResponse providerHistory,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        var mockQuery = new GetApprenticeshipQuery { Id = id };
        mockApiClient.Setup(client => client.Get<GetApprenticeshipChangeOfProviderChainResponse>(
                It.IsAny<GetApprenticeshipChangeOfProviderChainRequest>()))
            .ReturnsAsync(providerHistory);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.ChangeOfProviderChain.Should().BeEquivalentTo(providerHistory.ChangeOfProviderChain);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_ApprenticeshipDetails_And_TrainingProviderChain_Correctly(
        long id,
        SupportApprenticeshipDetails mockApiApprenticeshipDetailsResponse,
        GetApprenticeshipChangeOfProviderChainResponse mockApiChangeOfProviderChainResponse,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        var mockQuery = new GetApprenticeshipQuery { Id = id };
        var expectedApprenticeDetailsUrl = $"api/apprenticeships/{id}/approved-apprenticeship";
        mockApiClient.Setup(client => client.Get<SupportApprenticeshipDetails>(
                It.Is<GetApprovedApprenticeshipByIdRequest>(c =>
                    c.GetUrl == expectedApprenticeDetailsUrl)))
            .ReturnsAsync(mockApiApprenticeshipDetailsResponse);

        var expectedChangeOfProviderChainUrl = $"api/apprenticeships/{id}/change-of-provider-chain";
        mockApiClient.Setup(client => client.Get<GetApprenticeshipChangeOfProviderChainResponse>(
                It.Is<GetApprenticeshipChangeOfProviderChainRequest>(c =>
                    c.GetUrl == expectedChangeOfProviderChainUrl)))
            .ReturnsAsync(mockApiChangeOfProviderChainResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.ChangeOfProviderChain.Should().BeEquivalentTo(mockApiChangeOfProviderChainResponse.ChangeOfProviderChain);
    }

    [Test, MoqAutoData]
    public async Task Then_Calls_Api_Endpoints_With_Expected_Path_Correctly(
    long id,
    SupportApprenticeshipDetails mockApiApprenticeshipDetailsResponse,
    [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
    GetApprenticeshipQueryHandler sut)
    {
        var mockQuery = new GetApprenticeshipQuery { Id = id };

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        var expectedApprenticeDetailsUrl = $"api/apprenticeships/{id}/approved-apprenticeship";
        mockApiClient.Verify(client => client.Get<SupportApprenticeshipDetails>(
                It.Is<GetApprovedApprenticeshipByIdRequest>(c =>
                    c.GetUrl == expectedApprenticeDetailsUrl)));

        var expectedChangeOfProviderChainUrl = $"api/apprenticeships/{id}/change-of-provider-chain";
        mockApiClient.Verify(client => client.Get<GetApprenticeshipChangeOfProviderChainResponse>(
                It.Is<GetApprenticeshipChangeOfProviderChainRequest>(c =>
                    c.GetUrl == expectedChangeOfProviderChainUrl)));

        var expectedPendingUpdatesUrl = $"api/apprenticeships/{id}/updates";
        mockApiClient.Verify(client => client.Get<GetApprenticeshipPendingUpdatesResponse>(
                It.Is<GetApprenticeshipPendingUpdatesRequest>(c =>
                    c.GetUrl == expectedPendingUpdatesUrl)));

        var expectedTrainingDatesUrl = $"api/overlapping-training-date-request/{id}";
        mockApiClient.Verify(client => client.Get<GetApprenticeshipOverlappingTrainingDateResponse>(
                It.Is<GetApprenticeshipOverlappingTrainingDateRequest>(c =>
                    c.GetUrl == expectedTrainingDatesUrl)));

        var expectedPriceEpisodesUrl = $"api/apprenticeships/{id}/price-episodes";
        mockApiClient.Verify(client => client.Get<GetApprenticeshipPriceEpisodesResponse>(
                It.Is<GetApprenticeshipPriceEpisodeRequest>(c =>
                    c.GetUrl == expectedPriceEpisodesUrl)));
    }

    [Test, MoqAutoData]
    public async Task And_When_No_Apprenticeship_Exists_Then_Returns_Null(
    long id,
    [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
    GetApprenticeshipQueryHandler sut)
    {
        var mockQuery = new GetApprenticeshipQuery { Id = id };
        var expectedApprenticeDetailsUrl = $"api/apprenticeships/{id}/approved-apprenticeship";
        mockApiClient.Setup(client => client.Get<SupportApprenticeshipDetails>(
                It.IsAny<GetApprovedApprenticeshipByIdRequest>()))
            .ReturnsAsync((SupportApprenticeshipDetails)null);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Should().BeNull();
    }

    [TestCase(PaymentStatus.Active, "Live")]
    [TestCase(PaymentStatus.Paused, "Paused")]
    [TestCase(PaymentStatus.Withdrawn, "Stopped")]
    [TestCase(PaymentStatus.Completed, "Completed")]
    public void Then_Map_Past_Payment_Status_As_Expected(PaymentStatus status, string expectedDesc)
    {
        var text = GetApprenticeshipQueryHandler.MapPaymentStatus(status, DateTime.Now.AddMonths(-2));

        text.Should().Be(expectedDesc);
    }


    [TestCase(PaymentStatus.Active, "Waiting to start")]
    [TestCase(PaymentStatus.Paused, "Paused")]
    [TestCase(PaymentStatus.Withdrawn, "Stopped")]
    [TestCase(PaymentStatus.Completed, "Completed")]
    public void Then_Map_Future_Payment_Status_As_Expected(PaymentStatus status, string expectedDesc)
    {
        var text = GetApprenticeshipQueryHandler.MapPaymentStatus(status, DateTime.Now.AddMonths(2));

        text.Should().Be(expectedDesc);
    }

    [Test, MoqAutoData]
    public void And_When_OverlappingTrainingDates_Is_Null_Then_Returns_Null(
        long id,
        GetApprenticeshipOverlappingTrainingDateResponse response,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        response.OverlappingTrainingDateRequest = null;
        var actual = sut.MapToOverlappingTrainingDateRequest(response);

        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public void And_When_OverlappingTrainingDates_Has_No_PendingStatus_Then_Returns_Null(
        long id,
        GetApprenticeshipOverlappingTrainingDateResponse response,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        foreach (var a in response.OverlappingTrainingDateRequest)
        {
            a.Status = OverlappingTrainingDateRequestStatus.Rejected;
        }

        var actual = sut.MapToOverlappingTrainingDateRequest(response);

        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public void And_When_OverlappingTrainingDates_Has_A_PendingStatus_Then_Returns_ThatDate(
        long id,
        DateTime createdOn,
        GetApprenticeshipOverlappingTrainingDateResponse response,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        response.OverlappingTrainingDateRequest[0].Status = OverlappingTrainingDateRequestStatus.Pending;
        response.OverlappingTrainingDateRequest[0].CreatedOn = createdOn;

        var actual = sut.MapToOverlappingTrainingDateRequest(response);

        actual.Should().Be(createdOn);
    }

    [Test, MoqAutoData]
    public void And_When_PriceEpisodes_Is_Null_Then_Returns_Cost(
        decimal? cost,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        var actual = sut.GetPrice(null, cost);

        actual.Should().Be(cost);
    }

    [Test, MoqAutoData]
    public void And_When_NoPriceEpisodes_Exist_Then_Returns_Cost(
        decimal? cost,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        var actual = sut.GetPrice(new List<GetApprenticeshipPriceEpisodesResponse.PriceEpisode>(), cost);

        actual.Should().Be(cost);
    }

    [Test, MoqAutoData]
    public void And_PriceEpisodes_Exist_Then_Returns_First_Match(
        decimal cost,
        List<GetApprenticeshipPriceEpisodesResponse.PriceEpisode> priceEpisodes,
        [Frozen] Mock<IInternalApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetApprenticeshipQueryHandler sut)
    {
        priceEpisodes[0].FromDate = DateTime.Today.AddDays(-1);
        priceEpisodes[0].ToDate = null;
        priceEpisodes[0].Cost = cost;

        foreach (var pe in priceEpisodes.Skip(1))
        {
            pe.FromDate = DateTime.Today.AddYears(-2);
            pe.ToDate = DateTime.Today.AddYears(-1);
        }

        var actual = sut.GetPrice(priceEpisodes, cost+100);

        actual.Should().Be(cost);
    }
}