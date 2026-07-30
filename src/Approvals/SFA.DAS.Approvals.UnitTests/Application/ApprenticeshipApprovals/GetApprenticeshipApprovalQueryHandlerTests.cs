using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using static SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query.GetApprenticeshipApprovalResponse;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries;

[TestFixture]
public class GetApprenticeshipApprovalQueryHandlerTests
{
    private GetApprenticeshipApprovalQueryHandler _handler;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;

    private GetApprenticeshipApprovalResponse _apprenticeshipApproval;
    private GetApprenticeshipApprovalQuery _query;

    private GetTrainingProgrammeResponse _courseResponse;
    private List<TrainingProgrammeFundingPeriod> _fundingPeriods;
    private DateTime _apprenticeshipStartDate;
    private List<ChangeItem> _items;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();
        _apprenticeshipStartDate = new DateTime(2026, 1, 1);

        _items = new List<ChangeItem>();
        _items.Add(new ChangeItem { FieldName = "TNP1", NewValue = "1000", OldValue = "900" });
        _items.Add(new ChangeItem { FieldName = "TNP2", NewValue = "100", OldValue = "90" });

        _fundingPeriods = new List<TrainingProgrammeFundingPeriod>();
        _query = fixture.Create<GetApprenticeshipApprovalQuery>();
        _apprenticeshipApproval = fixture.Build<GetApprenticeshipApprovalResponse>().With(x=>x.AccountId, _query.EmployerAccountId)
            .With(x=>x.StartDate, _apprenticeshipStartDate)
            .With(x=>x.Items, _items).Create();
        _courseResponse = new GetTrainingProgrammeResponse
        {
            TrainingProgramme = fixture.Build<TrainingProgramme>().With(x => x.FundingPeriods, _fundingPeriods).Create()
        };

        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipApprovalResponse>(It.Is<GetApprenticeshipApprovalRequest>(
                    r => r.ApprenticeshipId == _query.ApprenticeshipId
                    && r.ApprovalRequestId == _query.ApprovalRequestId
                    )))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipApprovalResponse>(_apprenticeshipApproval, HttpStatusCode.OK, string.Empty));

        _apiClient.Setup(x=>x.Get<GetTrainingProgrammeResponse>(It.Is<GetTrainingProgrammeRequest>(
            r => r.CourseCode == _apprenticeshipApproval.CourseCode)))
            .ReturnsAsync(_courseResponse);

        _handler = new GetApprenticeshipApprovalQueryHandler(_apiClient.Object);
    }

    [Test]
    public async Task Handle_when_apprenticeshipApproval_is_returned()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_apprenticeshipApproval);
    }

    [TestCase(10000, "8000", "1000", false)]
    [TestCase(10000, "8000", "2000", false)]
    [TestCase(10000, "8001", "2000", true)]
    [TestCase(10000, "18000", "2000", true)]
    public async Task Handle_when_apprenticeshipApproval_is_returned_and_latest_funding_band_cap_is_used(int fundingBandCap, string tnp1NewValue, string tnp2NewValue, bool expectedFundingBandExceededStatus )
    {
        _fundingPeriods.Add(new TrainingProgrammeFundingPeriod
        {
            FundingCap = fundingBandCap,
            EffectiveFrom = _apprenticeshipStartDate.AddMonths(-1)
        });
        var tnp1 = _apprenticeshipApproval.Items.First(x => x.FieldName == "TNP1");
        tnp1.NewValue = tnp1NewValue;
        var tnp2 = _apprenticeshipApproval.Items.First(x => x.FieldName == "TNP2");
        tnp2.NewValue = tnp2NewValue;

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().NotBeNull();
        result.ExceedsFundingCap.Should().Be(expectedFundingBandExceededStatus);
    }

    [TestCase(10000, "8000", "1000", false)]
    [TestCase(10000, "8000", "2000", false)]
    [TestCase(10000, "8001", "2000", true)]
    [TestCase(10000, "18000", "2000", true)]
    public async Task Handle_when_apprenticeshipApproval_is_returned_and_a_single_funding_band_cap_is_used(int fundingBandCap, string tnp1NewValue, string tnp2NewValue, bool expectedFundingBandExceededStatus)
    {
        _fundingPeriods.Add(new TrainingProgrammeFundingPeriod
        {
            FundingCap = fundingBandCap,
            EffectiveFrom = _apprenticeshipStartDate.AddMonths(1)
        });
        var tnp1 = _apprenticeshipApproval.Items.First(x => x.FieldName == "TNP1");
        tnp1.NewValue = tnp1NewValue;
        var tnp2 = _apprenticeshipApproval.Items.First(x => x.FieldName == "TNP2");
        tnp2.NewValue = tnp2NewValue;

        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().NotBeNull();
        result.ExceedsFundingCap.Should().Be(expectedFundingBandExceededStatus);
    }


    [Test]
    public async Task Handle_when_apprenticeshipApproval_is_not_found()
    {
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipApprovalResponse>(It.Is<GetApprenticeshipApprovalRequest>(
                    r => r.ApprenticeshipId == _query.ApprenticeshipId
                    && r.ApprovalRequestId == _query.ApprovalRequestId
                    )))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipApprovalResponse>(null, HttpStatusCode.NotFound, string.Empty)); 
        
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().BeNull();
    }

    [Test]
    public async Task Handle_when_apprenticeshipApproval_is_returned_but_accountId_not_matching()
    {
        _query.EmployerAccountId++;
        var act = async () => await _handler.Handle(_query, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedAccessException>().WithMessage("This Employer does not have access to this apprenticeship approval.");
    }

    [Test]
    public async Task Handle_when_apprenticeshipApproval_returns_unexpected_response()
    {
        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipApprovalResponse>(It.Is<GetApprenticeshipApprovalRequest>(
                    r => r.ApprenticeshipId == _query.ApprenticeshipId
                    && r.ApprovalRequestId == _query.ApprovalRequestId
                    )))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipApprovalResponse>(null, HttpStatusCode.BadRequest, string.Empty));

        var act = async () => await _handler.Handle(_query, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage("An unexpected Status code was returned from the API.");
    }
}