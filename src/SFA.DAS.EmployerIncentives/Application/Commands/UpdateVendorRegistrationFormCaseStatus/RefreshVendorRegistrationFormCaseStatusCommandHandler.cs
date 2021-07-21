using MediatR;
using Microsoft.Extensions.Logging;
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
        private readonly IVendorRegistrationService _vendorRegistrationService;
        private readonly ILogger<RefreshVendorRegistrationFormCaseStatusCommandHandler> _logger;

        public RefreshVendorRegistrationFormCaseStatusCommandHandler(
            ICustomerEngagementFinanceService financeService,
            IVendorRegistrationService vendorRegistrationService,
            ILogger<RefreshVendorRegistrationFormCaseStatusCommandHandler> logger)
        {
            _financeService = financeService;
            _vendorRegistrationService = vendorRegistrationService;
            _logger = logger;
        }

        public async Task<DateTime> Handle(RefreshVendorRegistrationFormCaseStatusCommand request, CancellationToken cancellationToken)
        {
            var currentDateTime = DateTime.UtcNow;
            var toDateDateTime = request.FromDateTime.AddDays(1);
            request.ToDateTime = toDateDateTime;

            GetVendorRegistrationCaseStatusUpdateResponse response = null;
            var pageNo = 0;

            do
            {
                pageNo++;
                response = await GetUpdatesFromFinanceApi(request, response?.SkipCode);
                await SendUpdates(response);
            } while (HasNextPage(response) && pageNo < 5);

            var nextRunDateTime = toDateDateTime < currentDateTime ? toDateDateTime : currentDateTime;
            return await Task.FromResult(nextRunDateTime);
        }

        private static bool HasNextPage(GetVendorRegistrationCaseStatusUpdateResponse response)
        {
            return response != null && !string.IsNullOrEmpty(response.SkipCode);
        }

        private async Task SendUpdates(GetVendorRegistrationCaseStatusUpdateResponse response)
        {
            Task UpdateVendorRegistrationCaseStatus(VendorRegistrationCase @case)
            {
                return _vendorRegistrationService.UpdateVendorRegistrationCaseStatus(
                    new UpdateVendorRegistrationCaseStatusRequest
                    {
                        CaseId = @case.CaseId,
                        HashedLegalEntityId = @case.ApprenticeshipLegalEntityId,
                        Status = @case.CaseStatus,
                        CaseStatusLastUpdatedDate = @case.CaseStatusLastUpdatedDate
                    });
            }

            await Task.WhenAll(response.RegistrationCases
                .Where(c => !string.IsNullOrEmpty(c.ApprenticeshipLegalEntityId) && c.CaseType?.ToUpper() == "NEW")
                .Select(UpdateVendorRegistrationCaseStatus));
        }

        private async Task<GetVendorRegistrationCaseStatusUpdateResponse> GetUpdatesFromFinanceApi(RefreshVendorRegistrationFormCaseStatusCommand request, string skipCode)
        {
            _logger.LogInformation("[VRF Refresh] Requesting VRF Case status with parameters: [DateTimeFrom={FromDateTime}] [DateTimeTo={ToDateTime}] [SkipCode='{SkipCode}']", request.FromDateTime, request.ToDateTime, skipCode);

            var response = await _financeService.GetVendorRegistrationCasesByLastStatusChangeDate(request.FromDateTime, request.ToDateTime, skipCode);

            if (response?.RegistrationCases == null)
            {
                throw new ArgumentNullException($"[VRF Refresh] Error retrieving data from Finance API with parameters: [DateTimeFrom={request.FromDateTime} [DateTimeTo={request.ToDateTime}]");
            }

            _logger.LogInformation("[VRF Refresh] Number of VRF Case updates received from Finance API : [{Cases}]", response.RegistrationCases.Count);
            return response;
        }
    }
}