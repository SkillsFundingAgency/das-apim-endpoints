using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class UpdateVendorRegistrationCaseStatusCommandHandler : IRequestHandler<UpdateVendorRegistrationCaseStatusCommand, Unit>
    {
        private readonly ILegalEntitiesService _legalEntitiesService;
        private readonly IVendorRegistrationService _vendorRegistrationService;
        public UpdateVendorRegistrationCaseStatusCommandHandler(ILegalEntitiesService legalEntitiesService, IVendorRegistrationService vendorRegistrationService)
        {
            _legalEntitiesService = legalEntitiesService;
            _vendorRegistrationService = vendorRegistrationService;
        }

        public async Task<Unit> Handle(UpdateVendorRegistrationCaseStatusCommand request, CancellationToken cancellationToken)
        {
            var legalEntity = await _legalEntitiesService.GetLegalEntity(request.AccountId, request.AccountLegalEntityId);

            var updateRequest = new UpdateVendorRegistrationCaseStatusRequest
            {
                HashedLegalEntityId = legalEntity.HashedLegalEntityId,
                Status = request.VrfCaseStatus,
                CaseStatusLastUpdatedDate = DateTime.Now
            };
            await _vendorRegistrationService.UpdateVendorRegistrationCaseStatus(updateRequest);
            return await Task.FromResult(Unit.Value);
        }
    }
}
