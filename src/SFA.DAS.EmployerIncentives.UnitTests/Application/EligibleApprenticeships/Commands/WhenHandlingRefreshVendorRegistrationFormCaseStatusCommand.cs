using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingRefreshVendorRegistrationFormCaseStatusCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_legal_entities_are_updated_with_the_vendor_registration_form_Details(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            GetVendorRegistrationCaseStatusUpdateResponse vendorResponse,
            GetLatestVendorRegistrationCaseUpdateDateTimeResponse lastUpdateResponse
            )
        {
            var command = new RefreshVendorRegistrationFormCaseStatusCommand();
            vendorResponse.SkipCode = "";

            incentivesService.Setup(x => x.GetLatestVendorRegistrationCaseUpdateDateTime()).ReturnsAsync(lastUpdateResponse);

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value, null))
                .ReturnsAsync(vendorResponse);


            await handler.Handle(command, CancellationToken.None);

            foreach (var @case in vendorResponse.RegistrationCases)
            {
                incentivesService.Verify(
                    x => x.UpdateVendorRegistrationCaseStatus(It.Is<UpdateVendorRegistrationCaseStatusRequest>(r =>
                            r.CaseId == @case.CaseId &&
                            r.HashedLegalEntityId == @case.ApprenticeshipLegalEntityId &&
                            r.Status == @case.CaseStatus &&
                            r.CaseStatusLastUpdatedDate == @case.CaseStatusLastUpdatedDate)),
                    Times.Once());
            }
        }

        [Test, MoqAutoData]
        public async Task And_no_cases_returned_from_FinanceAPI_Then_The_legal_entities_are_not_updated(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            GetLatestVendorRegistrationCaseUpdateDateTimeResponse lastUpdateResponse)
        {
            incentivesService.Setup(x => x.GetLatestVendorRegistrationCaseUpdateDateTime()).ReturnsAsync(lastUpdateResponse);
            var command = new RefreshVendorRegistrationFormCaseStatusCommand();

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value, null))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse());

            await handler.Handle(command, CancellationToken.None);

            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Never());
        }

        [Test, MoqAutoData]
        public async Task Default_datetime_filter_is_used_for_the_initial_run(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler
            )
        {
            var lastUpdateResponse = new GetLatestVendorRegistrationCaseUpdateDateTimeResponse { LastUpdateDateTime = null }; // "first run"
            incentivesService.Setup(x => x.GetLatestVendorRegistrationCaseUpdateDateTime()).ReturnsAsync(lastUpdateResponse);
            var command = new RefreshVendorRegistrationFormCaseStatusCommand();

            var defaultDateTime = DateTime.Parse("01/08/2020", new CultureInfo("en-GB"));

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(defaultDateTime, null))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse());

            await handler.Handle(command, CancellationToken.None);

            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Never());
        }

        [Test, MoqAutoData]
        public async Task And_response_with_SkipCode_returned_from_FinanceAPI_Then_a_call_is_made_using_skiptoken_parameter(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            GetLatestVendorRegistrationCaseUpdateDateTimeResponse lastUpdateResponse,
            string skipCode)
        {
            incentivesService.Setup(x => x.GetLatestVendorRegistrationCaseUpdateDateTime()).ReturnsAsync(lastUpdateResponse);
            var command = new RefreshVendorRegistrationFormCaseStatusCommand();

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value, null))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse { SkipCode = skipCode });
            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value, skipCode))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse());

            await handler.Handle(command, CancellationToken.None);

            financeService.Verify(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value, null), Times.Once);
            financeService.Verify(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value, skipCode), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_paging_limit_is_enforced(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            RefreshVendorRegistrationFormCaseStatusCommand command,
            string skipCode)
        {

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse { SkipCode = skipCode });

            await handler.Handle(command, CancellationToken.None);

            financeService.Verify(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(It.IsAny<DateTime>(), It.IsAny<string>()), Times.Exactly(5));
        }
    }
}
