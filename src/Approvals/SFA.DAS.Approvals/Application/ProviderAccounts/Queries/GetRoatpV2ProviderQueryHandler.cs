using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Polly;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.ProviderAccounts.Queries;

public class GetRoatpV2ProviderQueryHandler(IRoatpV2TrainingProviderService roatpV2TrainingProviderService) : IRequestHandler<GetRoatpV2ProviderQuery, bool>
{
    public async Task<bool> Handle(GetRoatpV2ProviderQuery request, CancellationToken cancellationToken)
    {
        var retryPolicy = Policy
                 .Handle<Exception>()
                 .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        var result = await retryPolicy.ExecuteAsync(() => roatpV2TrainingProviderService.GetProviderSummary(request.Ukprn));

        return result != null && result.CanAccessApprenticeshipService;
    }
}