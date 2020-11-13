using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplications
{
    public class GetApplicationsHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsResult>
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public GetApplicationsHandler(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<GetApplicationsResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var response = await _employerIncentivesService.GetApprenticeApplications(request.AccountId, request.AccountLegalEntityId);

            return new GetApplicationsResult
            {
                ApprenticeApplications = response
            };
        }
    }
}
