using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplicationAccountLegalEntity
{ 
    public class GetApplicationAccountLegalEntityHandler : IRequestHandler<GetApplicationAccountLegalEntityQuery, long>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetApplicationAccountLegalEntityHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
             
        public async Task<long> Handle(GetApplicationAccountLegalEntityQuery request, CancellationToken cancellationToken)
        {
            return await _employerIncentivesService.GetApplicationLegalEntity(request.AccountId, request.ApplicationId);
        }
    }
}

