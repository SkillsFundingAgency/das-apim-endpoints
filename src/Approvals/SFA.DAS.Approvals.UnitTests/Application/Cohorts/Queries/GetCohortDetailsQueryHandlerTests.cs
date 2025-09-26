﻿using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Approvals.Application;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails;
using SFA.DAS.Approvals.Application.DraftApprenticeships.Queries.GetEditDraftApprenticeshipDeliveryModel;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.Approvals.Types;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;
using Party = SFA.DAS.Approvals.InnerApi.Responses.Party;
using Standard = SFA.DAS.Approvals.Types.Standard;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts;

[TestFixture]
public class GetCohortDetailsQueryHandlerTests
{
    private GetCohortDetailsQueryHandler _handler;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;
    private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
    private ServiceParameters _serviceParameters;

    private GetCohortResponse _cohort;
    private GetCohortDetailsQuery _query;

    private GetDraftApprenticeshipsResponse _draftApprenticeship;

    private GetApprenticeshipEmailOverlapResponse _emailOverlaps;

    private GetPriorLearningErrorResponse _rplErrors;


    private GetEditDraftApprenticeshipDeliveryModelQueryResult _queryEditDraftResult;
    private List<Standard> _providerStandards;

    private List<string> _deliveryModels;
    private Mock<IDeliveryModelService> _deliveryModelService;
    private Mock<IFjaaService> _fjaaService;
    private Mock<IProviderStandardsService> _providerCoursesService;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();

        _cohort = fixture.Build<GetCohortResponse>()
            .With(x => x.WithParty, Party.Employer)
            .With(x => x.IsLinkedToChangeOfPartyRequest, false)
            .Create();

        _query = fixture.Create<GetCohortDetailsQuery>();

        _queryEditDraftResult = fixture.Create<GetEditDraftApprenticeshipDeliveryModelQueryResult>();


        _draftApprenticeship = fixture.Create<GetDraftApprenticeshipsResponse>();

        _emailOverlaps = fixture.Create<GetApprenticeshipEmailOverlapResponse>();

        _rplErrors = fixture.Create<GetPriorLearningErrorResponse>();


        _deliveryModels = fixture.Create<List<string>>();

        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetCohortResponse>(It.Is<GetCohortRequest>(r => r.CohortId == _query.CohortId)))
            .ReturnsAsync(new ApiResponse<GetCohortResponse>(_cohort, HttpStatusCode.OK, string.Empty));


        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetDraftApprenticeshipsResponse>(It.Is<GetDraftApprenticeshipsRequest>(r => r.CohortId == _query.CohortId)))
            .ReturnsAsync(new ApiResponse<GetDraftApprenticeshipsResponse>(_draftApprenticeship, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipEmailOverlapResponse>(It.Is<GetApprenticeshipEmailOverlapRequest>(r => r.CohortId == _query.CohortId)))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipEmailOverlapResponse>(_emailOverlaps, HttpStatusCode.OK, string.Empty));


        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetPriorLearningErrorResponse>(It.Is<GetPriorLearningErrorRequest>(r => r.CohortId == _query.CohortId)))
            .ReturnsAsync(new ApiResponse<GetPriorLearningErrorResponse>(_rplErrors, HttpStatusCode.OK, string.Empty));


        _deliveryModelService = new Mock<IDeliveryModelService>();

        _fjaaService = new Mock<IFjaaService>();

        _serviceParameters = new ServiceParameters((Approvals.Application.Shared.Enums.Party)_cohort.WithParty, _cohort.AccountId);

        _providerStandards = fixture.Create<List<Standard>>();
        _providerCoursesService = new Mock<IProviderStandardsService>();
        _providerCoursesService.Setup(x => x.GetStandardsData(It.Is<long>(id => id == _cohort.ProviderId)))
            .ReturnsAsync(() => new ProviderStandardsData { Standards = _providerStandards });

        _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();

        _handler = new GetCohortDetailsQueryHandler(_apiClient.Object, _serviceParameters, _fjaaService.Object, _providerCoursesService.Object, _coursesApiClient.Object);
    }

    [Test]
    public async Task Handle_ProviderName_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.ProviderName.Should().Be(_cohort.ProviderName);
    }

    [Test]
    public async Task Handle_LegalEntityName_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.LegalEntityName.Should().Be(_cohort.LegalEntityName);
    }

    [Test]
    public async Task Handle_InvalidProviderCourseCodes_IsMapped_Empty()
    {
        _providerStandards.Clear();

        foreach (var courseCode in _draftApprenticeship.DraftApprenticeships.Select(x => x.CourseCode).Distinct())
        {
            _providerStandards.Add(new Standard(courseCode, $"test-{courseCode}"));
        }

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.InvalidProviderCourseCodes.Should().BeEquivalentTo(Enumerable.Empty<string>());
    }

    [Test]
    public async Task Handle_InvalidProviderCourseCodes_IsMapped_NotEmpty()
    {
        _providerStandards.Clear();

        var result = await _handler.Handle(_query, CancellationToken.None);

        var expected = _draftApprenticeship.DraftApprenticeships.Select(x => x.CourseCode).Distinct();

        expected.Should().BeEquivalentTo(result.InvalidProviderCourseCodes);
    }

    [Test]
    public async Task Handle_CohortId_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.CohortId.Should().Be(_cohort.CohortId);
    }

    [Test]
    public async Task Handle_CohortReference_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.CohortReference.Should().Be(_cohort.CohortReference);
    }

    [Test]
    public async Task Handle_AccountId_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.AccountId.Should().Be(_cohort.AccountId);
    }

    [Test]
    public async Task Handle_AccountLegalEntity_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.AccountLegalEntityId.Should().Be(_cohort.AccountLegalEntityId);
    }

    [Test]
    public async Task Handle_ProviderId_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.ProviderId.Should().Be(_cohort.ProviderId);
    }

        
    [Test]
    public async Task Handle_IsFundedByTransfer_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.IsFundedByTransfer.Should().Be(_cohort.IsFundedByTransfer);
    }
        
    [Test]
    public async Task Handle_TransferSenderId_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.TransferSenderId.Should().Be(_cohort.TransferSenderId);
    }
        
    [Test]
    public async Task Handle_PledgeApplicationId_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.PledgeApplicationId.Should().Be(_cohort.PledgeApplicationId);
    }
        
    [Test]
    public async Task Handle_WithParty_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.WithParty.Should().Be(_cohort.WithParty);
    }
        
    [Test]
    public async Task Handle_LatestMessageCreatedByEmployer_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.LatestMessageCreatedByEmployer.Should().Be(_cohort.LatestMessageCreatedByEmployer);
    }
        
    [Test]
    public async Task Handle_LatestMessageCreatedByProvider_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.LatestMessageCreatedByProvider.Should().Be(_cohort.LatestMessageCreatedByProvider);
    }
        
    [Test]
    public async Task Handle_IsApprovedByEmployer_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.IsApprovedByEmployer.Should().Be(_cohort.IsApprovedByEmployer);
    }
        
    [Test]
    public async Task Handle_IsApprovedByProvider_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.IsApprovedByProvider.Should().Be(_cohort.IsApprovedByProvider);
    }
        
    [Test]
    public async Task Handle_IsCompleteForEmployer_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.IsCompleteForEmployer.Should().Be(_cohort.IsCompleteForEmployer);
    }

    [Test]
    public async Task Handle_IsCompleteForProvider_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.IsCompleteForProvider.Should().Be(_cohort.IsCompleteForProvider);
    }
        
    [Test]
    public async Task Handle_LevyStatus_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.LevyStatus.Should().Be(_cohort.LevyStatus);
    }
        
    [Test]
    public async Task Handle_ChangeOfPartyRequestId_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.ChangeOfPartyRequestId.Should().Be(_cohort.ChangeOfPartyRequestId);
    }
        
    [Test]
    public async Task Handle_IsLinkedToChangeOfPartyRequest_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.IsLinkedToChangeOfPartyRequest.Should().Be(_cohort.IsLinkedToChangeOfPartyRequest);
    }
        
    [Test]
    public async Task Handle_TransferApprovalStatus_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.TransferApprovalStatus.Should().Be(_cohort.TransferApprovalStatus);
    }

    [Test]
    public async Task Handle_LastAction_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.LastAction.Should().Be(_cohort.LastAction);
    }

    [Test]
    public async Task Handle_ApprenticeEmailIsRequired_Is_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.ApprenticeEmailIsRequired.Should().Be(_cohort.ApprenticeEmailIsRequired);
    }

    [Test]
    public async Task Handle_DraftApprenticeships_Are_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.DraftApprenticeships.Should().BeEquivalentTo(_draftApprenticeship.DraftApprenticeships);
    }
        
    [Test]
    public async Task Handle_ApprenticeshipEmailOverlaps_Are_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.ApprenticeshipEmailOverlaps.Should().BeEquivalentTo(_emailOverlaps.ApprenticeshipEmailOverlaps);
    }

    [Test]
    public async Task Handle_RplErrorDraftApprenticeshipIds_Are_Mapped()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);
        result.RplErrorDraftApprenticeshipIds.Should().BeEquivalentTo(_rplErrors.DraftApprenticeshipIds);
    }

    [Test, MoqAutoData]
    public async Task Handle_HasFoundationApprenticeships_Is_True_When_Foundation_Apprenticeships_Found_From_API(
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient)
    {
        // Arrange
        var standard = new Standard("TEST1", "Test Standard 1");
        _providerStandards.Clear();
        _providerStandards.Add(standard);
        _draftApprenticeship.DraftApprenticeships.First().CourseCode = "TEST1";

        _coursesApiClient.Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == "TEST1")))
            .ReturnsAsync(new GetStandardsListItem { ApprenticeshipType = "FoundationApprenticeship" });

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.HasFoundationApprenticeships.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Handle_HasFoundationApprenticeships_Is_False_When_No_Foundation_Apprenticeships_Found_From_API(
        [Frozen] Mock<ICoursesApiClient<CoursesApiConfiguration>> coursesApiClient)
    {
        // Arrange
        var standard = new Standard("TEST1", "Test Standard 1");
        _providerStandards.Clear();
        _providerStandards.Add(standard);
        _draftApprenticeship.DraftApprenticeships.First().CourseCode = "TEST1";

        _coursesApiClient.Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == "TEST1")))
            .ReturnsAsync(new GetStandardsListItem { ApprenticeshipType = "StandardApprenticeship" });

        // Act
        var result = await _handler.Handle(_query, CancellationToken.None);

        // Assert
        result.HasFoundationApprenticeships.Should().BeFalse();
    }
}