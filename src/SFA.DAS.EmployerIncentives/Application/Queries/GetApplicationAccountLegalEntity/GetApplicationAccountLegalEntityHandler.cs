using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplicationAccountLegalEntity
{ 
    public class GetApplicationAccountLegalEntityHandler : IRequestHandler<GetApplicationAccountLegalEntityQuery, long>
    {
        private readonly IApplicationService _applicationService;

        public GetApplicationAccountLegalEntityHandler(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }
             
        public async Task<long> Handle(GetApplicationAccountLegalEntityQuery request, CancellationToken cancellationToken)
        {
            return await _applicationService.GetApplicationLegalEntity(request.AccountId, request.ApplicationId);
        }
    }
}

