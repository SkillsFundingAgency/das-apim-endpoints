using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Queries.GetApplications
{
    public class GetApplicationsHandler : IRequestHandler<GetApplicationsQuery, GetApplicationsResult>
    {
        private readonly IApplicationService _applicationService;

        public GetApplicationsHandler(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        public async Task<GetApplicationsResult> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
        {
            var response = await _applicationService.GetPaymentApplications(request.AccountId, request.AccountLegalEntityId);

            return new GetApplicationsResult
            {
                ApprenticeApplications = response.ApprenticeApplications,
                BankDetailsStatus = response.BankDetailsStatus,
                FirstSubmittedApplicationId = response.FirstSubmittedApplicationId
            };
        }
    }
}
