using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.ProviderAccounts.Queries
{
    public class GetRoatpV2ProviderQueryHandler(IRoatpV2TrainingProviderService roatpV2TrainingProviderService)
        : IRequestHandler<GetRoatpV2ProviderQuery, bool>
    {
        public async Task<bool> Handle(GetRoatpV2ProviderQuery request, CancellationToken cancellationToken)
        {
            var result = await roatpV2TrainingProviderService.GetProviderSummary(request.Ukprn);

            return result is { CanAccessApprenticeshipService: true };
        }
    }
}