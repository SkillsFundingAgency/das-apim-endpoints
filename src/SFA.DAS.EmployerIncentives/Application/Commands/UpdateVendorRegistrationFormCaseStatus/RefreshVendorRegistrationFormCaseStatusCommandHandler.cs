using MediatR;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus
{
    public class RefreshVendorRegistrationFormCaseStatusCommandHandler : IRequest
    {
        private readonly ICustomerEngagementFinanceService _financeService;
        private readonly IEmployerIncentivesService _incentivesService;

        public RefreshVendorRegistrationFormCaseStatusCommandHandler(ICustomerEngagementFinanceService financeService,
            IEmployerIncentivesService incentivesService)
        {
            _financeService = financeService;
            _incentivesService = incentivesService;
        }

        public async Task Handle(RefreshVendorRegistrationFormCaseStatusCommand command, CancellationToken none)
        {
            var response = await _financeService.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.ToDateTime);

            foreach (var c in response.RegistrationCases)
            {
                await _incentivesService.UpdateVendorRegistrationCaseStatus(
                    new UpdateVendorRegistrationCaseStatusRequest
                    {
                        CaseId = c.CaseId,
                        VendorId = c.SubmittedVendorIdentifier,
                        LegalEntityId = c.ApprenticeshipLegalEntityId,
                        Status = c.CaseStatus,
                        CaseStatusLastUpdatedDate = c.CaseStatusLastUpdatedDate
                    });
            }

        }
    }
}