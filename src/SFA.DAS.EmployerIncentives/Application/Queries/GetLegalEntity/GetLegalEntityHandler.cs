using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetLegalEntity
{
    public class GetLegalEntityHandler : IRequestHandler<GetLegalEntityQuery, GetLegalEntityResult>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetLegalEntityHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
        public async Task<GetLegalEntityResult> Handle(GetLegalEntityQuery request, CancellationToken cancellationToken)
        {
            var response = await _employerIncentivesService.GetLegalEntity(request.AccountId, request.AccountLegalEntityId);
            
            return new GetLegalEntityResult
            {
                AccountLegalEntity = response
            }; 
                
        }
    }
}