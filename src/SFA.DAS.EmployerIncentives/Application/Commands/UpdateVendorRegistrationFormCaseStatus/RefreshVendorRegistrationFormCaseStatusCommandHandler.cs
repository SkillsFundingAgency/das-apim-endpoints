using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Application.Commands.AddEmployerVendorId;
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
        private readonly IMediator _mediator;

        public RefreshVendorRegistrationFormCaseStatusCommandHandler(ICustomerEngagementFinanceService financeService,
            IEmployerIncentivesService incentivesService, ILogger<RefreshVendorRegistrationFormCaseStatusCommandHandler> logger,
            IMediator mediator)
        {
            _financeService = financeService;
            _incentivesService = incentivesService;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<DateTime> Handle(RefreshVendorRegistrationFormCaseStatusCommand request, CancellationToken cancellationToken)
        {
            var currentDateTime = DateTime.UtcNow;
            var toDateDateTime = request.FromDateTime.AddDays(1);

            request.ToDateTime = toDateDateTime;

            var response = await GetUpdatesFromFinanceApi(request);

            var nextRunDateTime = toDateDateTime < currentDateTime ? toDateDateTime : currentDateTime;

            if (!response.RegistrationCases.Any())
            {
                _logger.LogInformation($"[VRF Refresh] No cases returned by the Finance API with parameters: [DateTimeFrom={request.FromDateTime.ToIsoDateTime()}] [DateTimeTo={request.ToDateTime.ToIsoDateTime()}]", request.FromDateTime, request.ToDateTime);

                return await Task.FromResult(nextRunDateTime);
            }

            if (!string.IsNullOrEmpty(response.SkipCode))
            {
                _logger.LogError($"[VRF Refresh] [SkipCode={response.SkipCode}] returned by the Finance API with parameters: [DateTimeFrom={request.FromDateTime.ToIsoDateTime()}] [DateTimeTo={request.ToDateTime.ToIsoDateTime()}]", request.FromDateTime, request.ToDateTime);
            }

            FindLatestUpdateForEachLegalEntity(response);                      

            await Task.WhenAll(response.RegistrationCases.Select(UpdateVendorRegistrationCaseStatus));

            return await Task.FromResult(nextRunDateTime);
        }

        private async Task UpdateVendorRegistrationCaseStatus(VendorRegistrationCase @case)
        {
            var accountLegalEntity = await _incentivesService.GetLegalEntityByHashedId(@case.ApprenticeshipLegalEntityId);
            if (!VendorIdIsPopulated(accountLegalEntity?.VrfVendorId))
            {
                await _mediator.Send(new GetAndAddEmployerVendorIdCommand(@case.ApprenticeshipLegalEntityId));
            }

            await _incentivesService.UpdateVendorRegistrationCaseStatus(
                new UpdateVendorRegistrationCaseStatusRequest
                {
                    CaseId = @case.CaseId,
                    HashedLegalEntityId = @case.ApprenticeshipLegalEntityId,
                    Status = @case.CaseStatus,
                    CaseStatusLastUpdatedDate = @case.CaseStatusLastUpdatedDate
                });
        }

        private async Task<GetVendorRegistrationCaseStatusUpdateResponse> GetUpdatesFromFinanceApi(RefreshVendorRegistrationFormCaseStatusCommand request)
        {
            _logger.LogInformation($"[VRF Refresh] Requesting VRF Case status with parameters: [DateTimeFrom={request.FromDateTime.ToIsoDateTime()}] [DateTimeTo={request.ToDateTime.ToIsoDateTime()}]", request.FromDateTime, request.ToDateTime);

            var response = await _financeService.GetVendorRegistrationCasesByLastStatusChangeDate(request.FromDateTime, request.ToDateTime);

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

        private bool VendorIdIsPopulated(string vendorId)
        {
            if (String.IsNullOrWhiteSpace(vendorId))
            {
                return false;
            }

            if (vendorId == "000000")
            {
                return false;
            }

            return true;
        }
    }
}