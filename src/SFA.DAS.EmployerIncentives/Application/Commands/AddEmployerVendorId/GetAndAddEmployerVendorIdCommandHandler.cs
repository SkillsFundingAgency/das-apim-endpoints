using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerIncentives.Interfaces;

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

        public async Task<Unit> Handle(GetAndAddEmployerVendorIdCommand command, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Requesting Vendor By Apprenticeship Service LegalEntityId: {command.HashedLegalEntityId}");

            var employerVendorId = await GetEmployerVendorId(command.HashedLegalEntityId);

            await _incentivesService.AddEmployerVendorIdToLegalEntity(command.HashedLegalEntityId, employerVendorId);
            
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
                    throw new ApplicationException("Calling GetVendorByApprenticeshipLegalEntityId returned a null response");
                }

                if (response.Vendor == null)
                {
                    throw new ApplicationException("Calling GetVendorByApprenticeshipLegalEntityId returned a null response for Vendor");
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