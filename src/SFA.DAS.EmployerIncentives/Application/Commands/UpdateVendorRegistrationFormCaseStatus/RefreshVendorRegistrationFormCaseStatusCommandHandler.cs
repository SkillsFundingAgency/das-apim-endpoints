using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Extensions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class RefreshVendorRegistrationFormCaseStatusCommandHandler : IRequestHandler<RefreshVendorRegistrationFormCaseStatusCommand, DateTime>
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

        public async Task<DateTime> Handle(RefreshVendorRegistrationFormCaseStatusCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"[VRF Refresh] Requesting VRF Case status with parameters: [dateTimeFrom={request.FromDateTime.ToIsoDateTime()}]", request.FromDateTime);

            var response = await _financeService.GetVendorRegistrationCasesByLastStatusChangeDate(request.FromDateTime);

            if (response == null)
            {
                _logger.LogError("[VRF Refresh] Error retrieving data from Finance API with parameters: [dateTimeFrom={request.FromDateTime.ToIsoDateTime()}]", request.FromDateTime);
            }

            if (response.RegistrationCases?.Any() != true)
            {
                _logger.LogInformation($"[VRF Refresh] No cases returned by the Finance API with parameters: [dateTimeFrom={request.FromDateTime.ToIsoDateTime()}]", request.FromDateTime);
                return await Task.FromResult(request.FromDateTime);
            }

            _logger.LogInformation($"[VRF Refresh] Number of VRF Case updates received from Finance API : [{response.RegistrationCases.Count}]");

            GetLatestCasesForEachLegalEntity(response);

            _logger.LogInformation($"[VRF Refresh] Number of unique Legal Entities found: [{response.RegistrationCases.Count}]. Updating their VRF Case status...");

            Task UpdateVendorRegistrationCaseStatus(VendorRegistrationCase @case)
            {
                return _incentivesService.UpdateVendorRegistrationCaseStatus(
                    new UpdateVendorRegistrationCaseStatusRequest
                    {
                        CaseId = @case.CaseId,
                        HashedLegalEntityId = @case.ApprenticeshipLegalEntityId,
                        Status = @case.CaseStatus,
                        CaseStatusLastUpdatedDate = @case.CaseStatusLastUpdatedDate
                    });
            }

            await Task.WhenAll(response.RegistrationCases.Select(UpdateVendorRegistrationCaseStatus));

            var latestCaseUpdateDateTime = response.RegistrationCases
                .OrderBy(x => x.CaseStatusLastUpdatedDate).Last().CaseStatusLastUpdatedDate;

            return await Task.FromResult(latestCaseUpdateDateTime);
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