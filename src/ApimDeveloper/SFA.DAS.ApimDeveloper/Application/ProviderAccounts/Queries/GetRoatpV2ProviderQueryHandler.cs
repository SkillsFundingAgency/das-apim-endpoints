using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApimDeveloper.Application.ProviderAccounts.Queries
{
    public class GetRoatpV2ProviderQueryHandler : IRequestHandler<GetRoatpV2ProviderQuery, bool>
    {
        private readonly IRoatpV2TrainingProviderService _roatpV2TrainingProviderService;

        public GetRoatpV2ProviderQueryHandler(IRoatpV2TrainingProviderService roatpV2TrainingProviderService)
        {
            _roatpV2TrainingProviderService = roatpV2TrainingProviderService;
        }
        public async Task<bool> Handle(GetRoatpV2ProviderQuery request, CancellationToken cancellationToken)
        {
            var result = await _roatpV2TrainingProviderService.GetProviderSummary(request.Ukprn);

            return result != null && result.CanAccessApprenticeshipService;
        }
    }
}