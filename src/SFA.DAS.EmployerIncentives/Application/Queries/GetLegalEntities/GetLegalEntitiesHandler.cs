using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntities
{
    public class GetLegalEntitiesHandler : IRequestHandler<GetLegalEntitiesQuery, GetLegalEntitiesResult>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetLegalEntitiesHandler (IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
        public async Task<GetLegalEntitiesResult> Handle(GetLegalEntitiesQuery request, CancellationToken cancellationToken)
        {
            var response = await _employerIncentivesService.GetAccountLegalEntities(request.AccountId);
            
            return new GetLegalEntitiesResult
            {
                AccountLegalEntities = response.AccountLegalEntities
            }; 
                
        }
    }
}