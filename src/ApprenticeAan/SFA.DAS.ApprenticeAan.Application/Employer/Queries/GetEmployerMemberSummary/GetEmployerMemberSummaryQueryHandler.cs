using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Employer.Queries.GetEmployerMemberSummary;

public class GetEmployerMemberSummaryQueryHandler : IRequestHandler<GetEmployerMemberSummaryQuery, GetEmployerMemberSummaryQueryResult>
{
    private readonly ICommitmentsV2InnerApiClient _commitmentsV2ApiClient;

    public GetEmployerMemberSummaryQueryHandler(ICommitmentsV2InnerApiClient commitmentsV2ApiClient)
    {
        _commitmentsV2ApiClient = commitmentsV2ApiClient;
    }

    public async Task<GetEmployerMemberSummaryQueryResult> Handle(GetEmployerMemberSummaryQuery request, CancellationToken cancellationToken)
    {
        var responseEmployerAccountTask = _commitmentsV2ApiClient.GetEmployerAccountSummary(request.EmployerAccountId, cancellationToken);
        var responseEmployerSummaryTask = _commitmentsV2ApiClient.GetApprenticeshipsSummaryForEmployer(request.EmployerAccountId, cancellationToken);

        await Task.WhenAll(responseEmployerAccountTask, responseEmployerSummaryTask);

        GetEmployerMemberSummaryQueryResult result = new();

        var outputEmployerAccount = responseEmployerAccountTask.Result;

        result.ActiveCount = outputEmployerAccount!.ApprenticeshipStatusSummaryResponse.Any() ?
            outputEmployerAccount.ApprenticeshipStatusSummaryResponse.FirstOrDefault()!.ActiveCount : 0;

        var outputEmployerSummary = responseEmployerSummaryTask.Result!;

        if (outputEmployerSummary.Sectors.Any())
            result.Sectors = outputEmployerSummary.Sectors;

        return result;
    }
}
