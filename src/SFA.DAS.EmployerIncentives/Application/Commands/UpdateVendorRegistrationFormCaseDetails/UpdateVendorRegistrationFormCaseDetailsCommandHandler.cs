using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseDetails
{
    public class UpdateVendorRegistrationFormCaseDetailsCommandHandler : IRequestHandler<UpdateVendorRegistrationFormCaseDetailsCommand>
    {
        private readonly ICustomerEngagementFinanceService _financeService;
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public UpdateVendorRegistrationFormCaseDetailsCommandHandler(ICustomerEngagementFinanceService financeService, IEmployerIncentivesService employerIncentivesService)
        {
            _financeService = financeService;
            _employerIncentivesService = employerIncentivesService;
        }

        public async Task<Unit> Handle(UpdateVendorRegistrationFormCaseDetailsCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}