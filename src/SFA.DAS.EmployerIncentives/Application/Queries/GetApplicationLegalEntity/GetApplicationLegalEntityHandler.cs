using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Application.Queries.GetApplication;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplicationLegalEntity
{ 
    public class GetApplicationLegalEntityHandler : IRequestHandler<GetApplicationLegalEntityQuery, long>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetApplicationLegalEntityHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
             
        public async Task<long> Handle(GetApplicationLegalEntityQuery request, CancellationToken cancellationToken)
        {
            return await _employerIncentivesService.GetApplicationLegalEntity(request.AccountId, request.ApplicationId);
        }
    }
}

