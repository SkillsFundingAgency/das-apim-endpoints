using MediatR;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Recruit.Application.Queries.GetProvider
{
    public class GetProviderQueryHandler : IRequestHandler<GetProviderQuery, GetProviderQueryResult>
    {
        private readonly ITrainingProviderService _trainingProviderService;

        public GetProviderQueryHandler(ITrainingProviderService trainingProviderService)
        {
            _trainingProviderService = trainingProviderService;
        }

        public async Task<GetProviderQueryResult> Handle(GetProviderQuery request, CancellationToken cancellationToken)
        {
            var response = await _trainingProviderService.GetTrainingProviderDetails(request.Ukprn);

            return new GetProviderQueryResult
            {
                Id = response.Id,
                LegalName = response.LegalName,
                TradingName = response.TradingName,
                Ukprn = response.Ukprn,
                ProviderType = new ProviderTypeResponse
                {
                    Id = response.ProviderType.Id,
                }
            };
        }
    }
}