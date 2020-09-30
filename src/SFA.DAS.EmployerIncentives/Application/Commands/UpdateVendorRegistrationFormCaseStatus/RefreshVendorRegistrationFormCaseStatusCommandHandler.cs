using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class RefreshVendorRegistrationFormCaseStatusCommandHandler : IRequestHandler<RefreshVendorRegistrationFormCaseStatusCommand>
    {
        private readonly ICustomerEngagementFinanceService _financeService;
        private readonly IEmployerIncentivesService _incentivesService;
        private readonly ILogger<RefreshVendorRegistrationFormCaseStatusCommandHandler> _logger;

        public RefreshVendorRegistrationFormCaseStatusCommandHandler(ICustomerEngagementFinanceService financeService,
            IEmployerIncentivesService incentivesService, ILogger<RefreshVendorRegistrationFormCaseStatusCommandHandler> logger)
        {
            _financeService = financeService;
            _incentivesService = incentivesService;
            _logger = logger;
        }

        public async Task<Unit> Handle(RefreshVendorRegistrationFormCaseStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Requesting VRF Case status updates from: [{request.FromDateTime}] to: [{request.ToDateTime}]");

            var response = await _financeService.GetVendorRegistrationCasesByLastStatusChangeDate(request.FromDateTime, request.ToDateTime);

            if (response == null)
            {
                _logger.LogError("Error retrieving data from Finance API");
            }

            _logger.LogInformation($"Number of VRF Cases received from Finance API: [{response.RegistrationCases.Count}]");

            GetLatestCasesForEachLegalEntity(response);

            _logger.LogInformation($"Number of unique Legal Entities found: [{response.RegistrationCases.Count}]. Updating their VRF Case status...");

            Task UpdateVendorRegistrationCaseStatus(VendorRegistrationCase @case)
            {
                return _incentivesService.UpdateVendorRegistrationCaseStatus(
                    new UpdateVendorRegistrationCaseStatusRequest
                    {
                        CaseId = @case.CaseId,
                        VendorId = @case.SubmittedVendorIdentifier,
                        HashedLegalEntityId = @case.ApprenticeshipLegalEntityId,
                        Status = @case.CaseStatus,
                        CaseStatusLastUpdatedDate = @case.CaseStatusLastUpdatedDate
                    });
            }

            await Task.WhenAll(response.RegistrationCases.Select(UpdateVendorRegistrationCaseStatus));
            return Unit.Value;
        }

        private static void GetLatestCasesForEachLegalEntity(GetVendorRegistrationCaseStatusUpdateResponse response)
        {
            var filtered = response.RegistrationCases.Where(c => !string.IsNullOrEmpty(c.ApprenticeshipLegalEntityId)
                                                            && c.CaseType?.ToUpper() == "NEW");

            response.RegistrationCases = filtered.GroupBy(x => x.ApprenticeshipLegalEntityId,
                    (_, g) => g.OrderByDescending(e => e.CaseStatusLastUpdatedDate).First())
                .ToList();
        }
    }
}