using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.Onboarding.ConfirmDetails.Queries;

public class GetOnboardingConfirmDetailsQueryHandler(ICommitmentsV2ApiClient commitmentsV2ApiClient)
    : IRequestHandler<GetOnboardingConfirmDetailsQuery, GetOnboardingConfirmDetailsQueryResult>
{
    public async Task<GetOnboardingConfirmDetailsQueryResult> Handle(GetOnboardingConfirmDetailsQuery request, CancellationToken cancellationToken)
    {
        var responseEmployerAccountTask = commitmentsV2ApiClient.GetEmployerAccountSummary(request.EmployerAccountId, cancellationToken);
        var responseEmployerSummaryTask = commitmentsV2ApiClient.GetApprenticeshipsSummaryForEmployer(request.EmployerAccountId, cancellationToken);

        await Task.WhenAll(responseEmployerAccountTask, responseEmployerSummaryTask);

        GetOnboardingConfirmDetailsQueryResult result = new();

        var outputEmployerAccount = responseEmployerAccountTask.Result;

        result.NumberOfActiveApprentices = outputEmployerAccount!.ApprenticeshipStatusSummaryResponse.Any() ?
            outputEmployerAccount.ApprenticeshipStatusSummaryResponse.Sum(a => a.ActiveCount) : 0;

        var outputEmployerSummary = responseEmployerSummaryTask.Result!;

        result.Sectors = outputEmployerSummary.Sectors.ToList();

        return result;
    }
}