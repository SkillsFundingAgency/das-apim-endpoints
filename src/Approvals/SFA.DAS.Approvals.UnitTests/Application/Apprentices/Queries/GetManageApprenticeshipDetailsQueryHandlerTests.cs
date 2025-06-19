﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apprenticeships.Types;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Apprentices.Queries.Apprenticeship.GetManageApprenticeshipDetails;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetApprenticeshipKey;
using SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Apprenticeships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.CollectionCalendar;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using GetApprenticeshipUpdatesResponse = SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses.GetApprenticeshipUpdatesResponse;
using GetPendingPriceChangeRequest = SFA.DAS.Approvals.InnerApi.ApprenticeshipsApi.GetPendingPriceChange.GetPendingPriceChangeRequest;
using Party = SFA.DAS.Approvals.Application.Shared.Enums.Party;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries;

[TestFixture]
public class GetManageApprenticeshipDetailsQueryHandlerTests
{
    private GetManageApprenticeshipDetailsQueryHandler _handler;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
    private Mock<IDeliveryModelService> _deliveryModelService;
    private ServiceParameters _serviceParameters;
    private Mock<ILearningApiClient<ApprenticeshipsApiConfiguration>> _apprenticeshipsApiClient;
    private Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>> _collectionCalendarApiClient;

    private GetApprenticeshipResponse _apprenticeship;
    private GetManageApprenticeshipDetailsQuery _query;
    private List<string> _deliveryModels;
    private GetPriceEpisodesResponse _priceEpisodesResponse;
    private GetApprenticeshipUpdatesResponse _apprenticeshipUpdatesResponse;
    private GetDataLocksResponse _dataLockStatusResponse;
    private GetChangeOfPartyRequestsResponse _changeOfPartyRequestsResponse;
    private GetChangeOfProviderChainResponse _changeOfProviderChainResponse;
    private GetChangeOfEmployerChainResponse _changeOfEmployerChainResponse;
    private GetOverlappingTrainingDateResponse _overlappingTrainingDateResponse;
    private GetPendingPriceChangeResponse _pendingPriceChangeResponse;
    private GetPendingStartDateChangeApiResponse _pendingStartDateChangeResponse;
    private GetPaymentStatusApiResponse _paymentStatusResponse;
    private GetLearnerStatusResponse _learnerStatusResponse;
    private Guid _apprenticeshipKey;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();

        _query = fixture.Create<GetManageApprenticeshipDetailsQuery>();
        _apprenticeship = fixture.Build<GetApprenticeshipResponse>()
            .With(x => x.EmployerAccountId, 123)
            .With(x => x.Id, _query.ApprenticeshipId)
            .With(x => x.ActualStartDate, (DateTime?)null)
            .With(x => x.IsOnFlexiPaymentPilot, true)
            .Create();

        _priceEpisodesResponse = fixture.Create<GetPriceEpisodesResponse>();
        _apprenticeshipUpdatesResponse = fixture.Create<GetApprenticeshipUpdatesResponse>();
        _dataLockStatusResponse = fixture.Create<GetDataLocksResponse>();
        _changeOfPartyRequestsResponse = fixture.Create<GetChangeOfPartyRequestsResponse>();
        _changeOfProviderChainResponse = fixture.Create<GetChangeOfProviderChainResponse>();
        _changeOfEmployerChainResponse = fixture.Create<GetChangeOfEmployerChainResponse>();
        _overlappingTrainingDateResponse = fixture.Create<GetOverlappingTrainingDateResponse>();
        _pendingPriceChangeResponse = fixture.Create<GetPendingPriceChangeResponse>();
        _pendingStartDateChangeResponse = fixture.Create<GetPendingStartDateChangeApiResponse>();
        _paymentStatusResponse = fixture.Create<GetPaymentStatusApiResponse>();
        _learnerStatusResponse = fixture.Create<GetLearnerStatusResponse>();

        _deliveryModels = fixture.Create<List<string>>();

        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetPriceEpisodesResponse>(It.Is<GetPriceEpisodesRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetPriceEpisodesResponse>(_priceEpisodesResponse, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(It.Is<GetApprenticeshipUpdatesRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipUpdatesResponse>(_apprenticeshipUpdatesResponse, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetDataLocksResponse>(It.Is<GetDataLocksRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetDataLocksResponse>(_dataLockStatusResponse, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetChangeOfPartyRequestsResponse>(It.Is<GetChangeOfPartyRequestsRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetChangeOfPartyRequestsResponse>(_changeOfPartyRequestsResponse, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetChangeOfProviderChainResponse>(It.Is<GetChangeOfProviderChainRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetChangeOfProviderChainResponse>(_changeOfProviderChainResponse, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetChangeOfEmployerChainResponse>(It.Is<GetChangeOfEmployerChainRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetChangeOfEmployerChainResponse>(_changeOfEmployerChainResponse, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetOverlappingTrainingDateResponse>(It.Is<GetOverlappingTrainingDateRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetOverlappingTrainingDateResponse>(_overlappingTrainingDateResponse, HttpStatusCode.OK, string.Empty));

        _deliveryModelService = new Mock<IDeliveryModelService>();

        _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _apprenticeship.ProviderId),
                It.Is<string>(s => s == _apprenticeship.CourseCode),
                It.Is<long>(ale => ale == _apprenticeship.AccountLegalEntityId),
                It.Is<long?>(a => a == _apprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);

        _serviceParameters = new ServiceParameters(Party.Employer, 123);

        _apprenticeshipsApiClient = new Mock<ILearningApiClient<ApprenticeshipsApiConfiguration>>();

        _apprenticeshipKey = Guid.NewGuid();
        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<Guid>(It.Is<GetLearningKeyRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId))).ReturnsAsync(new ApiResponse<Guid>(_apprenticeshipKey, HttpStatusCode.OK, string.Empty));

        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<GetPendingPriceChangeResponse>(It.Is<GetPendingPriceChangeRequest>(r => r.LearningKey == _apprenticeshipKey)))
            .ReturnsAsync(new ApiResponse<GetPendingPriceChangeResponse>(_pendingPriceChangeResponse, HttpStatusCode.OK, string.Empty));

        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<GetPendingStartDateChangeApiResponse>(It.Is<GetPendingStartDateChangeRequest>(r => r.LearningKey == _apprenticeshipKey)))
            .ReturnsAsync(new ApiResponse<GetPendingStartDateChangeApiResponse>(_pendingStartDateChangeResponse, HttpStatusCode.OK, string.Empty));

        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<GetPaymentStatusApiResponse>(It.Is<GetPaymentStatusRequest>(r => r.LearningKey == _apprenticeshipKey)))
            .ReturnsAsync(new ApiResponse<GetPaymentStatusApiResponse>(_paymentStatusResponse, HttpStatusCode.OK, string.Empty));

        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<GetLearnerStatusResponse>(It.Is<GetLearnerStatusRequest>(r => r.LearningKey == _apprenticeshipKey)))
            .ReturnsAsync(new ApiResponse<GetLearnerStatusResponse>(_learnerStatusResponse, HttpStatusCode.OK, string.Empty));

        _collectionCalendarApiClient = new Mock<ICollectionCalendarApiClient<CollectionCalendarApiConfiguration>>();

        _handler = new GetManageApprenticeshipDetailsQueryHandler(_apiClient.Object, _deliveryModelService.Object, _serviceParameters, _apprenticeshipsApiClient.Object, _collectionCalendarApiClient.Object);
    }

    [TestCase(0, false)]
    [TestCase(1, false)]
    [TestCase(2, true)]
    public async Task Handle_HasMultipleDeliveryModelOptions_Reflects_Number_Of_Options_Available(int optionCount, bool expectedHasMultiple)
    {
        var fixture = new Fixture();
        _deliveryModels.Clear();
        _deliveryModels.AddRange(fixture.CreateMany<string>(optionCount));
        
        _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _apprenticeship.ProviderId),
                It.Is<string>(s => s == _apprenticeship.CourseCode),
                It.Is<long>(ale => ale == _apprenticeship.AccountLegalEntityId),
                It.Is<long?>(a => a == _apprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);

        var result = await _handler.Handle(_query, CancellationToken.None);
        result.HasMultipleDeliveryModelOptions.Should().Be(expectedHasMultiple);
    }

    [Test]
    public async Task Handle_Returns_objects_From_Commitments_Api()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        
        result.Apprenticeship.Should().Be(_apprenticeship);
        result.PriceEpisodes.Should().BeEquivalentTo(_priceEpisodesResponse.PriceEpisodes);
        result.ApprenticeshipUpdates.Should().BeEquivalentTo(_apprenticeshipUpdatesResponse.ApprenticeshipUpdates);
        result.ChangeOfPartyRequests.Should().BeEquivalentTo(_changeOfPartyRequestsResponse.ChangeOfPartyRequests);
        result.ChangeOfProviderChain.Should().BeEquivalentTo(_changeOfProviderChainResponse.ChangeOfProviderChain);
        result.ChangeOfEmployerChain.Should().BeEquivalentTo(_changeOfEmployerChainResponse.ChangeOfEmployerChain);
        result.OverlappingTrainingDateRequest.Should().BeEquivalentTo(_overlappingTrainingDateResponse.OverlappingTrainingDateRequest);
    }

    [Test]
    public async Task When_apprenticeship_has_pending_price_change_then_details_returned()
    {
        _pendingPriceChangeResponse.HasPendingPriceChange = true;
        
        var result = await _handler.Handle(_query, CancellationToken.None);
        
        result.PendingPriceChange.Cost.Should().Be(_pendingPriceChangeResponse.PendingPriceChange.PendingTotalPrice);
        result.PendingPriceChange.TrainingPrice.Should().Be(_pendingPriceChangeResponse.PendingPriceChange.PendingTrainingPrice);
        result.PendingPriceChange.EndPointAssessmentPrice.Should().Be(_pendingPriceChangeResponse.PendingPriceChange.PendingAssessmentPrice);
        result.PendingPriceChange.ProviderApprovedDate.Should().Be(_pendingPriceChangeResponse.PendingPriceChange.ProviderApprovedDate);
        result.PendingPriceChange.EmployerApprovedDate.Should().Be(_pendingPriceChangeResponse.PendingPriceChange.EmployerApprovedDate);
    }

    [Test]
    public async Task When_apprenticeship_has_pending_start_date_change_then_details_returned()
    {
        _pendingStartDateChangeResponse.HasPendingStartDateChange = true;
        
        var result = await _handler.Handle(_query, CancellationToken.None);
        
        result.PendingStartDateChange.PendingActualStartDate.Should().Be(_pendingStartDateChangeResponse.PendingStartDateChange.PendingActualStartDate);
        result.PendingStartDateChange.ProviderApprovedDate.Should().Be(_pendingStartDateChangeResponse.PendingStartDateChange.ProviderApprovedDate);
        result.PendingStartDateChange.EmployerApprovedDate.Should().Be(_pendingStartDateChangeResponse.PendingStartDateChange.EmployerApprovedDate);
        result.PendingStartDateChange.Initiator.Should().Be(_pendingStartDateChangeResponse.PendingStartDateChange.Initiator);
    }

    [Test]
    public async Task Handle_Returns_Correct_CanActualStartDateBeChanged_CurrentAcademicYear()
    {
        // Arrange
        var actualStartDate = DateTime.Now.Date.AddDays(-41); // start date within qualifying period
        var currentAcademicYearStartDate = actualStartDate.AddMonths(-2);
        _apprenticeship.ActualStartDate = actualStartDate;

        var currentAcademicYearResponse = new GetAcademicYearsResponse
        {
            StartDate = currentAcademicYearStartDate
        };
        _collectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByDateRequest>(r => r._dateTime == DateTime.Now.ToString("yyyy-MM-dd"))))
            .ReturnsAsync(currentAcademicYearResponse);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.CanActualStartDateBeChanged.Should().BeTrue();
    }

    [Test]
    public async Task Handle_Returns_Correct_CanActualStartDateBeChanged_PreviousOpenAcademicYear()
    {
        // Arrange
        var actualStartDate = DateTime.Now.Date.AddDays(-41); // start date within qualifying period
        var previousAcademicYearHardCloseDate = DateTime.Now.AddMonths(1); // previous year not hard closed yet
        var previousAcademicYearStartDate = DateTime.Now.AddMonths(-13);
        var currentAcademicYearStartDate = previousAcademicYearHardCloseDate.AddDays(1);
        _apprenticeship.ActualStartDate = actualStartDate;

        var currentAcademicYearResponse = new GetAcademicYearsResponse
        {
            StartDate = currentAcademicYearStartDate
        };
        var previousAcademicYearResponse = new GetAcademicYearsResponse
        {
            StartDate = previousAcademicYearStartDate,
            HardCloseDate = previousAcademicYearHardCloseDate
        };

        _collectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByDateRequest>(r => r._dateTime == DateTime.Now.ToString("yyyy-MM-dd"))))
            .ReturnsAsync(currentAcademicYearResponse);
       
        _collectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByDateRequest>(r => r._dateTime == DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"))))
            .ReturnsAsync(previousAcademicYearResponse);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.CanActualStartDateBeChanged.Should().BeTrue();
    }

    [Test]
    public async Task Handle_Returns_Correct_CanActualStartDateBeChanged_PreviousClosedAcademicYear()
    {
        // Arrange
        // Intentionally using dates in the past to assert that the test will work forever
        // Here the previous academic year is closed, again doesn't matter that the hard close date is years later the api will control returning correct data, but we need the hard close date to be BEFORE DateTime.Now for this scenario
        var actualStartDate = DateTime.Now.Date.AddDays(-41);
        _apprenticeship.ActualStartDate = actualStartDate;

        var currentAcademicYearStartDate = actualStartDate.AddYears(1);
        var previousAcademicYearStartDate = actualStartDate.AddMonths(-13);
        var previousAcademicYearHardCloseDate = DateTime.Now.AddMonths(-1);

        var currentAcademicYearResponse = new GetAcademicYearsResponse
        {
            StartDate = currentAcademicYearStartDate
        };
        var previousAcademicYearResponse = new GetAcademicYearsResponse
        {
            StartDate = previousAcademicYearStartDate,
            HardCloseDate = previousAcademicYearHardCloseDate
        };

        _collectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByDateRequest>(r => r._dateTime == DateTime.Now.ToString("yyyy-MM-dd"))))
            .ReturnsAsync(currentAcademicYearResponse);
      
        _collectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByDateRequest>(r => r._dateTime == DateTime.Now.AddYears(-1).ToString("yyyy-MM-dd"))))
            .ReturnsAsync(previousAcademicYearResponse);

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.CanActualStartDateBeChanged.Should().BeFalse();
    }

    [TestCase(41, true)] 
    [TestCase(42, true)] 
    [TestCase(43, false)]
    public async Task Handle_Returns_Correct_CanActualStartDateBeChanged_QualifyingPeriodPassed(int daysSinceApprenticeshipStartDate, bool expectedValue)
    {
        // Arrange
        var startOfToday = DateTime.Now.Date;
        var actualStartDate = startOfToday.AddDays(-daysSinceApprenticeshipStartDate);
        _apprenticeship.ActualStartDate = actualStartDate;

        var currentAcademicYearStartDate = actualStartDate.AddMonths(-3);
        var currentAcademicYearResponse = new GetAcademicYearsResponse
        {
            StartDate = currentAcademicYearStartDate
        };

        _collectionCalendarApiClient
            .Setup(x => x.Get<GetAcademicYearsResponse>(It.Is<GetAcademicYearByDateRequest>(r => r._dateTime == DateTime.Now.ToString("yyyy-MM-dd"))))
            .ReturnsAsync(currentAcademicYearResponse);
      
        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.CanActualStartDateBeChanged.Should().Be(expectedValue);
    }

    [Test]
    public async Task When_apprenticeship_has_no_pending_price_change_then_details_not_returned()
    {
        _pendingPriceChangeResponse.HasPendingPriceChange = false;
        
        var result = await _handler.Handle(_query, CancellationToken.None);
        
        result.PendingPriceChange.Should().BeNull();
    }

    [Test]
    public async Task Handle_Returns_null_if_apprenticeship_not_found()
    {
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(null, HttpStatusCode.NotFound, string.Empty));

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().BeNull();
    }

    [TestCase(true)]
    [TestCase(false)]
    public async Task When_apprenticeship_has_payments_frozen_then_correct_response_returned(bool paymentsFrozen)
    {
        _paymentStatusResponse.PaymentsFrozen = paymentsFrozen;
        
            _paymentStatusResponse.ReasonFrozen = "Test reason";
            _paymentStatusResponse.FrozenOn = DateTime.Now;


        //  Act
        var result = await _handler.Handle(_query, CancellationToken.None);
        
        result.PaymentsStatus.PaymentsFrozen.Should().Be(paymentsFrozen);
        result.PaymentsStatus.ReasonFrozen.Should().Be(_paymentStatusResponse.ReasonFrozen);
        result.PaymentsStatus.FrozenOn.Should().Be(_paymentStatusResponse.FrozenOn);
    }
    
    [Test]
    public async Task When_Apprenticeship_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _apprenticeship = null;
        
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipResponse>(It.Is<GetApprenticeshipRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipResponse>(_apprenticeship, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().ThrowAsync<ResourceNotFoundException>();
    }

    [Test]
    public async Task When_PriceEpisodeResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _priceEpisodesResponse = null;
        
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetPriceEpisodesResponse>(It.Is<GetPriceEpisodesRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetPriceEpisodesResponse>(_priceEpisodesResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
        
    [Test]
    public async Task When_ApprenticeshipUpdatesResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _apprenticeshipUpdatesResponse = null;
        
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(It.Is<GetApprenticeshipUpdatesRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipUpdatesResponse>(_apprenticeshipUpdatesResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
        
    [Test]
    public async Task When_DataLocksResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _dataLockStatusResponse = null;
        
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetDataLocksResponse>(It.Is<GetDataLocksRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetDataLocksResponse>(_dataLockStatusResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
        
    [Test]
    public async Task When_ChangeOfPartyResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _changeOfPartyRequestsResponse = null;
        
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetChangeOfPartyRequestsResponse>(It.Is<GetChangeOfPartyRequestsRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetChangeOfPartyRequestsResponse>(_changeOfPartyRequestsResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
        
    [Test]
    public async Task When_ChangeOfProviderChainResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _changeOfProviderChainResponse = null;
        
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetChangeOfProviderChainResponse>(It.Is<GetChangeOfProviderChainRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetChangeOfProviderChainResponse>(_changeOfProviderChainResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
        
    [Test]
    public async Task When_ChangeOfEmployerChainResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _changeOfEmployerChainResponse = null;
        
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetChangeOfEmployerChainResponse>(It.Is<GetChangeOfEmployerChainRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetChangeOfEmployerChainResponse>(_changeOfEmployerChainResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task When_OverlappingTrainingDatesResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _overlappingTrainingDateResponse = null;
        
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetOverlappingTrainingDateResponse>(It.Is<GetOverlappingTrainingDateRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)))
            .ReturnsAsync(new ApiResponse<GetOverlappingTrainingDateResponse>(_overlappingTrainingDateResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task When_DeliveryModelResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _deliveryModels = null;
        
        _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _apprenticeship.ProviderId),
                It.Is<string>(s => s == _apprenticeship.CourseCode),
                It.Is<long>(ale => ale == _apprenticeship.AccountLegalEntityId),
                It.Is<long?>(a => a == _apprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task When_DeliveryModelResponse_Is_Null_Then_HasMultipleDeliveryModelOptions_IsFalse()
    {
        _deliveryModels = null;
        
        _deliveryModelService.Setup(x => x.GetDeliveryModels(
                It.Is<long>(p => p == _apprenticeship.ProviderId),
                It.Is<string>(s => s == _apprenticeship.CourseCode),
                It.Is<long>(ale => ale == _apprenticeship.AccountLegalEntityId),
                It.Is<long?>(a => a == _apprenticeship.ContinuationOfId)))
            .ReturnsAsync(_deliveryModels);
        
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.HasMultipleDeliveryModelOptions.Should().BeFalse();
    }
    
    [Test]
    public async Task When_PendingPriceChangeResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _pendingPriceChangeResponse = null;
        
        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<GetPendingPriceChangeResponse>(It.Is<GetPendingPriceChangeRequest>(r => r.LearningKey == _apprenticeshipKey)))
            .ReturnsAsync(new ApiResponse<GetPendingPriceChangeResponse>(_pendingPriceChangeResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task When_PendingStartDateChangeResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _pendingStartDateChangeResponse = null;
        
        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<GetPendingStartDateChangeApiResponse>(It.Is<GetPendingStartDateChangeRequest>(r => r.LearningKey == _apprenticeshipKey)))
            .ReturnsAsync(new ApiResponse<GetPendingStartDateChangeApiResponse>(_pendingStartDateChangeResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);

        await result.Should().NotThrowAsync();
    }

    [Test]
    public async Task When_PaymentStatusResponse_Is_Null_Then_No_Exception_Is_Thrown()
    {
        _paymentStatusResponse = null;
        
        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<GetPaymentStatusApiResponse>(It.Is<GetPaymentStatusRequest>(r => r.LearningKey == _apprenticeshipKey)))
            .ReturnsAsync(new ApiResponse<GetPaymentStatusApiResponse>(_paymentStatusResponse, HttpStatusCode.OK, string.Empty));
        
        var result = async () => await _handler.Handle(_query, CancellationToken.None);
        
        await result.Should().NotThrowAsync();
    }
    
    [Test]
    public async Task When_not_found_returned_when_getting_payments_status_then_correct_response_returned()
    {
        //  Arrange
        _apprenticeshipsApiClient.Setup(x => x.GetWithResponseCode<GetPaymentStatusApiResponse>(It.IsAny<GetPaymentStatusRequest>()))
            .ReturnsAsync(new ApiResponse<GetPaymentStatusApiResponse>(null, HttpStatusCode.NotFound, string.Empty));
       
        //  Act
        var result = await _handler.Handle(_query, CancellationToken.None);
        
        result.PaymentsStatus.PaymentsFrozen.Should().BeFalse();
        result.PaymentsStatus.ReasonFrozen.Should().BeNull();
        result.PaymentsStatus.FrozenOn.Should().BeNull();
    }

    [Test]
    public async Task Handle_If_Not_On_PaymentsSimplificationPilot_Do_Not_Call_ApprenticeshipsApi_Subsequently()
    {
        _apprenticeship.IsOnFlexiPaymentPilot = false;

        var result = await _handler.Handle(_query, CancellationToken.None);

        _apprenticeshipsApiClient.Verify(
            x => x.GetWithResponseCode<Guid>(
                It.Is<GetLearningKeyRequest>(r => r.ApprenticeshipId == _query.ApprenticeshipId)), Times.Never);

        _apprenticeshipsApiClient.Verify(
            x => x.GetWithResponseCode<GetPendingPriceChangeResponse>(
                It.IsAny<GetPendingPriceChangeRequest>()), Times.Never);

        _apprenticeshipsApiClient.Verify(x =>
            x.GetWithResponseCode<GetPendingStartDateChangeApiResponse>(
                It.IsAny<GetPendingStartDateChangeRequest>()), Times.Never);

        _apprenticeshipsApiClient.Verify(
            x => x.GetWithResponseCode<GetPaymentStatusApiResponse>(
                It.IsAny<GetPaymentStatusRequest>()), Times.Never);

        _apprenticeshipsApiClient.Verify(
            x => x.GetWithResponseCode<GetLearnerStatusResponse>(
                It.IsAny<GetLearnerStatusRequest>()), Times.Never);

        result.LearnerStatusDetails.Should().NotBeNull();
        result.LearnerStatusDetails.LearnerStatus.Should().Be(LearnerStatus.None);
        result.PaymentsStatus.Should().NotBeNull();
        result.PaymentsStatus.PaymentsFrozen.Should().BeFalse();
    }
}