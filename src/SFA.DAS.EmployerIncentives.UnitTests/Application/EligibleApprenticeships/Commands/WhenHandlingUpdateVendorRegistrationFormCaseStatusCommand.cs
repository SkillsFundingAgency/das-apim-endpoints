using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingUpdateVendorRegistrationFormCaseStatusCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_legal_entity_is_updated_with_the_vendor_registration_form_Details(
            long legalEntityId,
            string caseId,
            string vendorId,
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            UpdateVendorRegistrationFormCaseStatusCommandHandler handler,
            GetVendorRegistrationStatusByCaseIdResponse vendorResponse)
        {
            financeService.Setup(x => x.GetVendorRegistrationStatusByCaseId(caseId)).ReturnsAsync(vendorResponse);

            var command = new UpdateVendorRegistrationFormCaseStatusCommand(legalEntityId, caseId, vendorId);

            await handler.Handle(command, CancellationToken.None);

            incentivesService.Verify(x => x.UpdateVendorRegistrationFormDetails(legalEntityId, It.Is<UpdateVendorRegistrationFormRequest>(r => r.CaseId == caseId && r.CaseStatus == vendorResponse.RegistrationCase.CaseStatus && r.VendorId == vendorId)), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_if_the_vendor_is_not_found_then_the_details_are_not_updated(
            long legalEntityId,
            string caseId,
            string vendorId,
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            UpdateVendorRegistrationFormCaseStatusCommandHandler handler)
        {
            financeService.Setup(x => x.GetVendorRegistrationStatusByCaseId(caseId)).ReturnsAsync((GetVendorRegistrationStatusByCaseIdResponse)null);

            var command = new UpdateVendorRegistrationFormCaseStatusCommand(legalEntityId, caseId, vendorId);

            await handler.Handle(command, CancellationToken.None);

            incentivesService.Verify(x => x.UpdateVendorRegistrationFormDetails(It.IsAny<long>(), It.IsAny<UpdateVendorRegistrationFormRequest>()), Times.Never);
        }
    }
}