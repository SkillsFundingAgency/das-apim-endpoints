using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities
{
    public class GetLegalEntitiesHandler : IRequestHandler<GetLegalEntitiesQuery, GetLegalEntitiesResult>
    {
        private readonly ILegalEntitiesService _legalEntitiesService;

        public GetLegalEntitiesHandler (ILegalEntitiesService legalEntitiesService)
        {
            _legalEntitiesService = legalEntitiesService;
        }
        public async Task<GetLegalEntitiesResult> Handle(GetLegalEntitiesQuery request, CancellationToken cancellationToken)
        {
            var response = await _legalEntitiesService.GetAccountLegalEntities(request.AccountId);
            
            return new GetLegalEntitiesResult
            {
                AccountLegalEntities = response
            }; 
                
        }
    }
}