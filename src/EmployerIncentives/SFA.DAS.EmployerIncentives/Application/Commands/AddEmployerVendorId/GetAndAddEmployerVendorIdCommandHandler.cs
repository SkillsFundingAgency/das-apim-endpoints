using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.AddEmployerVendorId
{
    public class GetAndAddEmployerVendorIdCommandHandler : IRequestHandler<GetAndAddEmployerVendorIdCommand, Unit>
    {
        private readonly ICustomerEngagementFinanceService _financeService;
        private readonly IVendorRegistrationService _vendorRegistrationService;
        private readonly ILogger<GetAndAddEmployerVendorIdCommandHandler> _logger;

        public GetAndAddEmployerVendorIdCommandHandler(
            ICustomerEngagementFinanceService financeService,
            IVendorRegistrationService vendorRegistrationService, 
            ILogger<GetAndAddEmployerVendorIdCommandHandler> logger)
        {
            _financeService = financeService;
            _vendorRegistrationService = vendorRegistrationService;
            _logger = logger;
        }

        public async Task<Unit> Handle(GetAndAddEmployerVendorIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Requesting VendorId for LegalEntityId: [{LegalEntityId}]", request.HashedLegalEntityId);

            var employerVendorId = await GetEmployerVendorId(request.HashedLegalEntityId);

            if (!string.IsNullOrEmpty(employerVendorId))
            {
                _logger.LogInformation("Received VendorId '{VendorId}' for LegalEntityId [{LegalEntityId}]", employerVendorId, request.HashedLegalEntityId);
                await _vendorRegistrationService.AddEmployerVendorIdToLegalEntity(request.HashedLegalEntityId, employerVendorId);
            }

            return Unit.Value;
        }

        private async Task<string> GetEmployerVendorId(string hashedLegalEntityId)
        {
            try
            {
                _logger.LogInformation("Calling GetVendorByApprenticeshipLegalEntityId for LegalEntityId [{LegalEntityId}]", hashedLegalEntityId);

                var response = await _financeService.GetVendorByApprenticeshipLegalEntityId(hashedLegalEntityId);

                if (response == null)
                {
                    throw new ArgumentException("Calling GetVendorByApprenticeshipLegalEntityId for LegalEntityId [{LegalEntityId}] returned a null response", hashedLegalEntityId);
                }

                if (response.Vendor == null)
                {
                    throw new ArgumentException("Calling GetVendorByApprenticeshipLegalEntityId for LegalEntityId [{LegalEntityId}] returned a null response for Vendor", hashedLegalEntityId);
                }

                if (response.Vendor != null && string.IsNullOrEmpty(response.Vendor.VendorIdentifier))
                {
                    _logger.LogInformation("No VendorId received for LegalEntityId: [{LegalEntityId}]. Error message: [{Error}]", hashedLegalEntityId, response.Vendor.ErrorText);
                }

                return response.Vendor.VendorIdentifier;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Problem calling GetVendorByApprenticeshipLegalEntityId for LegalEntityId: [{LegalEntityId}]", hashedLegalEntityId);
                throw;
            }
        }
    }
}