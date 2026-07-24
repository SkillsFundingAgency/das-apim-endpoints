using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.Approvals.Application.ApprenticeshipApprovals.Query;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Queries;

[TestFixture]
public class GetApprenticeshipApprovalQueryHandlerTests
{
    private GetApprenticeshipApprovalQueryHandler _handler;
    private Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> _apiClient;

    private GetApprenticeshipApprovalResponse _apprenticeshipApproval;
    private GetApprenticeshipApprovalQuery _query;

    [SetUp]
    public void Setup()
    {
        var fixture = new Fixture();

        _query = fixture.Create<GetApprenticeshipApprovalQuery>();
        _apprenticeshipApproval = fixture.Build<GetApprenticeshipApprovalResponse>().With(x=>x.AccountId, _query.EmployerAccountId).Create();

        _apiClient = new Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>>();

        _apiClient.Setup(x =>
                x.GetWithResponseCode<GetApprenticeshipApprovalResponse>(It.Is<GetApprenticeshipApprovalRequest>(
                    r => r.ApprenticeshipId == _query.ApprenticeshipId
                    && r.ApprovalRequestId == _query.ApprovalRequestId
                    )))
            .ReturnsAsync(new ApiResponse<GetApprenticeshipApprovalResponse>(_apprenticeshipApproval, HttpStatusCode.OK, string.Empty));

        _handler = new GetApprenticeshipApprovalQueryHandler(_apiClient.Object);
    }

    [Test]
    public async Task Handle_when_apprenticeshipApproval_is_returned()
    {
        var result = await _handler.Handle(_query, CancellationToken.None);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_apprenticeshipApproval);
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