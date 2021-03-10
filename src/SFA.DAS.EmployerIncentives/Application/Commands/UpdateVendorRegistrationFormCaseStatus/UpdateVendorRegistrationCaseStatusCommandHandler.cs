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
        private readonly IEmployerIncentivesService _incentivesService;
        public UpdateVendorRegistrationCaseStatusCommandHandler(IEmployerIncentivesService incentivesService)
        {
            _incentivesService = incentivesService;
        }

        public async Task<Unit> Handle(UpdateVendorRegistrationCaseStatusCommand request, CancellationToken cancellationToken)
        {
            var legalEntity = await _incentivesService.GetLegalEntity(request.AccountId, request.AccountLegalEntityId);

            var updateRequest = new UpdateVendorRegistrationCaseStatusRequest
            {
                HashedLegalEntityId = legalEntity.HashedLegalEntityId,
                Status = request.VrfCaseStatus,
                CaseStatusLastUpdatedDate = DateTime.Now
            };
            await _incentivesService.UpdateVendorRegistrationCaseStatus(updateRequest);
            return await Task.FromResult(Unit.Value);
        }
    }
}
