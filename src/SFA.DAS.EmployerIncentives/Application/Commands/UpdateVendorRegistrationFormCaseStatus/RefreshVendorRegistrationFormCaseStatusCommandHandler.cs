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
    public class RefreshVendorRegistrationFormCaseStatusCommandHandler : IRequest
    {
        private readonly ICustomerEngagementFinanceService _financeService;
        private readonly IEmployerIncentivesService _incentivesService;
        private readonly ILogger _logger;

        public RefreshVendorRegistrationFormCaseStatusCommandHandler(ICustomerEngagementFinanceService financeService,
            IEmployerIncentivesService incentivesService, ILogger logger)
        {
            _financeService = financeService;
            _incentivesService = incentivesService;
            _logger = logger;
        }

        public async Task Handle(RefreshVendorRegistrationFormCaseStatusCommand command, CancellationToken none)
        {
            _logger.LogInformation($"Requesting VRF Case status updates from: [{command.FromDateTime}] to: [{command.ToDateTime}]");

            var response = await _financeService.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.ToDateTime);

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
        }

        private static void GetLatestCasesForEachLegalEntity(GetVendorRegistrationCaseStatusUpdateResponse response)
        {
            var filtered = response.RegistrationCases.Where(c => !string.IsNullOrEmpty(c.ApprenticeshipLegalEntityId));

            response.RegistrationCases = filtered.GroupBy(x => x.ApprenticeshipLegalEntityId,
                    (_, g) => g.OrderByDescending(e => e.CaseStatusLastUpdatedDate).First())
                .ToList();
        }
    }
}