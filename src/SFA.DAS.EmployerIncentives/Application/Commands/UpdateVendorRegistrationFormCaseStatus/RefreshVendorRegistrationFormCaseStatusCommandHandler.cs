using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;
using System.Globalization;
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

        public RefreshVendorRegistrationFormCaseStatusCommandHandler(
            ICustomerEngagementFinanceService financeService,
            IEmployerIncentivesService incentivesService,
            ILogger<RefreshVendorRegistrationFormCaseStatusCommandHandler> logger)
        {
            _financeService = financeService;
            _incentivesService = incentivesService;
            _logger = logger;
        }

        public async Task<Unit> Handle(RefreshVendorRegistrationFormCaseStatusCommand request, CancellationToken cancellationToken)
        {
            var lastUpdateDateTime = await GetLastUpdateDateTime();
            GetVendorRegistrationCaseStatusUpdateResponse response = null;

            do
            {
                response = await GetUpdatesFromFinanceApi(lastUpdateDateTime, response?.SkipCode);
                await SendUpdates(response);
            } while (response != null && !string.IsNullOrEmpty(response.SkipCode));

            return Unit.Value;
        }

        private async Task SendUpdates(GetVendorRegistrationCaseStatusUpdateResponse response)
        {
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

            await Task.WhenAll(response.RegistrationCases
                .Where(c => !string.IsNullOrEmpty(c.ApprenticeshipLegalEntityId))
                .Select(UpdateVendorRegistrationCaseStatus));
        }

        private async Task<DateTime> GetLastUpdateDateTime()
        {
            var response = await _incentivesService.GetLatestVendorRegistrationCaseUpdateDateTime();

            return response.LastUpdateDateTime ?? DateTime.SpecifyKind(DateTime.Parse("01/08/2020", new CultureInfo("en-GB")), DateTimeKind.Utc);
        }

        private async Task<GetVendorRegistrationCaseStatusUpdateResponse> GetUpdatesFromFinanceApi(DateTime fromDateTime, string skipCode)
        {
            _logger.LogInformation("[VRF Refresh] Requesting VRF Case status with parameters: [DateTimeFrom: {fromDateTime}]", fromDateTime);

            var response = await _financeService.GetVendorRegistrationCasesByLastStatusChangeDate(fromDateTime, skipCode);

            if (response?.RegistrationCases == null)
            {
                _logger.LogError("[VRF Refresh] Error retrieving data from Finance API with parameters: [DateTimeFrom: {fromDateTime}, SkipCode: {SkipCode}]", fromDateTime, skipCode);
                return response;
            }

            _logger.LogInformation("[VRF Refresh] {Cases} cases returned by the Finance API with parameters: [DateTimeFrom: {fromDateTime}, SkipCode: {SkipCode}]",
                response.RegistrationCases.Count, fromDateTime, skipCode);

            return response;
        }
    }
}