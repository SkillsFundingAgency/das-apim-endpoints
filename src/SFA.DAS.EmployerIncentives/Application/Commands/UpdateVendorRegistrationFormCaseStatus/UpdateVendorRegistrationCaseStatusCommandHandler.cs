using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class UpdateVendorRegistrationCaseStatusCommandHandler : IRequestHandler<UpdateVendorRegistrationCaseStatusCommand>
    {
        private readonly ILegalEntitiesService _legalEntitiesService;
        private readonly IEmployerIncentivesService _employerIncentivesService;
        public UpdateVendorRegistrationCaseStatusCommandHandler(ILegalEntitiesService legalEntitiesService, IEmployerIncentivesService employerIncentivesService)
        {
            _legalEntitiesService = legalEntitiesService;
            _employerIncentivesService = employerIncentivesService;
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
            await _employerIncentivesService.UpdateVendorRegistrationCaseStatus(updateRequest);
            return await Task.FromResult(Unit.Value);
        }
    }
}
