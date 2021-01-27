using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.AddEmployerVendorId
{
    public class GetAndAddEmployerVendorIdCommandHandler : IRequestHandler<GetAndAddEmployerVendorIdCommand>
    {
        private readonly ICustomerEngagementFinanceService _financeService;
        private readonly IEmployerIncentivesService _incentivesService;
        private readonly ILogger<GetAndAddEmployerVendorIdCommandHandler> _logger;

        private const string CompanyName = "ESFA";

        public GetAndAddEmployerVendorIdCommandHandler(ICustomerEngagementFinanceService financeService,
            IEmployerIncentivesService incentivesService, ILogger<GetAndAddEmployerVendorIdCommandHandler> logger)
        {
            _financeService = financeService;
            _incentivesService = incentivesService;
            _logger = logger;
        }

        public async Task<Unit> Handle(GetAndAddEmployerVendorIdCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Requesting Vendor By Apprenticeship Service LegalEntityId: {request.HashedLegalEntityId}");

            var employerVendorId = await GetEmployerVendorId(request.HashedLegalEntityId);

            if (!string.IsNullOrEmpty(employerVendorId))
                await _incentivesService.AddEmployerVendorIdToLegalEntity(request.HashedLegalEntityId, employerVendorId);

            return Unit.Value;
        }

        private async Task<string> GetEmployerVendorId(string hashedLegalEntityId)
        {
            try
            {
                _logger.LogInformation("Calling GetVendorByApprenticeshipLegalEntityId with [{companyName}] and [{hashedLegalEntityId}]");

                var response = await _financeService.GetVendorByApprenticeshipLegalEntityId(CompanyName, hashedLegalEntityId);

                _logger.LogInformation("Called GetVendorByApprenticeshipLegalEntityId with [{companyName}] and [{hashedLegalEntityId}]");

                if (response == null)
                {
                    throw new ArgumentException("Calling GetVendorByApprenticeshipLegalEntityId returned a null response");
                }

                if (response.Vendor == null)
                {
                    throw new ArgumentException("Calling GetVendorByApprenticeshipLegalEntityId returned a null response for Vendor");
                }

                if (response.Vendor != null && string.IsNullOrEmpty(response.Vendor.VendorIdentifier))
                {
                    _logger.LogInformation($"No VendorId received for LegalEntityId: {hashedLegalEntityId}. Error message: {response.Vendor.ErrorText}");
                }

                return response.Vendor.VendorIdentifier;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Problem calling GetVendorByApprenticeshipLegalEntityId with [{CompanyName}] and [{hashedLegalEntityId}] ");
                throw;
            }
        }
    }
}