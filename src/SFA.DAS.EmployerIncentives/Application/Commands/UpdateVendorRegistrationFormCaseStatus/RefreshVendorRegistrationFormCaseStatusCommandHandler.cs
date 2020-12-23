using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Extensions;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;
using System.Collections.Generic;
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
            var currentDateTime = DateTime.UtcNow;
            var toDateDateTime = request.FromDateTime.AddDays(1);
            request.ToDateTime = toDateDateTime;
            var nextRunDateTime = toDateDateTime < currentDateTime ? toDateDateTime : currentDateTime;

            var updates = new List<VendorRegistrationCase>();
            GetVendorRegistrationCaseStatusUpdateResponse response = null;

            do
            {
                response = await GetUpdatesFromFinanceApi(request, response?.SkipCode);
                updates.AddRange(response.RegistrationCases);
            } while (!string.IsNullOrEmpty(response.SkipCode));


            if (response.RegistrationCases.Count == 0)
            {
                _logger.LogInformation($"[VRF Refresh] No cases returned by the Finance API with parameters: [DateTimeFrom={request.FromDateTime.ToIsoDateTime()}] [DateTimeTo={request.ToDateTime.ToIsoDateTime()}]", request.FromDateTime, request.ToDateTime);

                return await Task.FromResult(nextRunDateTime);
            }

            FindLatestUpdateForEachLegalEntity(response);

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

            if (!string.IsNullOrEmpty(response.SkipCode))
            {
                _logger.LogInformation($"[VRF Refresh] [SkipCode={response.SkipCode}] returned by the Finance API with parameters: [DateTimeFrom={request.FromDateTime.ToIsoDateTime()}] [DateTimeTo={request.ToDateTime.ToIsoDateTime()}]", request.FromDateTime, request.ToDateTime);

            }

            return await Task.FromResult(nextRunDateTime);
        }

        private async Task<GetVendorRegistrationCaseStatusUpdateResponse> GetUpdatesFromFinanceApi(RefreshVendorRegistrationFormCaseStatusCommand request, string skipCode)
        {
            _logger.LogInformation($"[VRF Refresh] Requesting VRF Case status with parameters: [DateTimeFrom={request.FromDateTime.ToIsoDateTime()}] [DateTimeTo={request.ToDateTime.ToIsoDateTime()}]", request.FromDateTime, request.ToDateTime);

            var response = await _financeService.GetVendorRegistrationCasesByLastStatusChangeDate(request.FromDateTime, request.ToDateTime, skipCode);

            if (response?.RegistrationCases == null)
            {
                _logger.LogError($"[VRF Refresh] Error retrieving data from Finance API with parameters: [DateTimeFrom={request.FromDateTime.ToIsoDateTime()} [DateTimeTo={request.ToDateTime.ToIsoDateTime()}]", request.FromDateTime, request.ToDateTime);
                throw new ArgumentNullException($"[VRF Refresh] Error retrieving data from Finance API with parameters: [DateTimeFrom={request.FromDateTime.ToIsoDateTime()} [DateTimeTo={request.ToDateTime.ToIsoDateTime()}]");
            }

            return response;
        }

        private void FindLatestUpdateForEachLegalEntity(GetVendorRegistrationCaseStatusUpdateResponse response)
        {
            _logger.LogInformation($"[VRF Refresh] Number of VRF Case updates received from Finance API : [{response.RegistrationCases.Count}]");

            var filtered = response.RegistrationCases.Where(c => !string.IsNullOrEmpty(c.ApprenticeshipLegalEntityId));

            response.RegistrationCases = filtered.GroupBy(x => x.ApprenticeshipLegalEntityId,
                    (_, g) => g.OrderByDescending(e => e.CaseStatusLastUpdatedDate).First())
                .ToList();

            _logger.LogInformation($"[VRF Refresh] Number of unique Legal Entities found: [{response.RegistrationCases.Count}]. Updating their VRF Case status...");
        }

    }
}