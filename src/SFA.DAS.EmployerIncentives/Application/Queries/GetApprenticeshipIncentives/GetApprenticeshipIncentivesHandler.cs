using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApprenticeshipIncentives
{
    public class GetApprenticeshipIncentivesHandler : IRequestHandler<GetApprenticeshipIncentivesQuery, GetApprenticeshipIncentivesResult>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetApprenticeshipIncentivesHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
        public async Task<GetApprenticeshipIncentivesResult> Handle(GetApprenticeshipIncentivesQuery request, CancellationToken cancellationToken)
        {
            var response = await _employerIncentivesService.GetApprenticeshipIncentives(request.AccountId, request.AccountLegalEntityId);
            
            return new GetApprenticeshipIncentivesResult
            {
                ApprenticeshipIncentives = response
            }; 
                
        }
    }
}