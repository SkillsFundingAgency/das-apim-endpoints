using AutoFixture;
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
using System.Linq;
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
        public async Task Then_the_Vrf_case_status_is_updated_once_for_each_legal_entity_using_the_latest_case_details(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            GetLatestVendorRegistrationCaseUpdateDateTimeResponse lastUpdateResponse
            )
        {

            var fixture = new Fixture();
            incentivesService.Setup(x => x.GetLatestVendorRegistrationCaseUpdateDateTime()).ReturnsAsync(lastUpdateResponse);

            var cases = fixture.CreateMany<VendorRegistrationCase>(6).ToList();

            cases[0].ApprenticeshipLegalEntityId = null;

            cases[1].ApprenticeshipLegalEntityId = "XYZ123";
            cases[1].CaseStatusLastUpdatedDate = DateTime.Parse("02-01-2000", new CultureInfo("en-GB"));

            cases[2].ApprenticeshipLegalEntityId = "ABCDEF";
            cases[2].CaseStatusLastUpdatedDate = DateTime.Parse("01-01-2000", new CultureInfo("en-GB"));

            cases[3].ApprenticeshipLegalEntityId = "XYZ123";
            cases[3].CaseStatusLastUpdatedDate = DateTime.Parse("13-01-2000", new CultureInfo("en-GB")); // max date

            cases[4].ApprenticeshipLegalEntityId = "XYZ123";
            cases[4].CaseStatusLastUpdatedDate = DateTime.Parse("04-01-2000", new CultureInfo("en-GB"));

            cases[5].ApprenticeshipLegalEntityId = "";

            var financeApiResponse = new GetVendorRegistrationCaseStatusUpdateResponse { RegistrationCases = cases };

            var command = new RefreshVendorRegistrationFormCaseStatusCommand();

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value, null))
                .ReturnsAsync(financeApiResponse);

            await handler.Handle(command, CancellationToken.None);


            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.Is<UpdateVendorRegistrationCaseStatusRequest>(r => string.IsNullOrEmpty(r.HashedLegalEntityId))), Times.Never);
            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Exactly(2));

            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(
                    It.Is<UpdateVendorRegistrationCaseStatusRequest>(r =>
                        r.CaseId == cases[2].CaseId &&
                        r.HashedLegalEntityId == cases[2].ApprenticeshipLegalEntityId &&
                        r.Status == cases[2].CaseStatus &&
                        r.CaseStatusLastUpdatedDate == cases[2].CaseStatusLastUpdatedDate)), Times.Once);

            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(
                    It.Is<UpdateVendorRegistrationCaseStatusRequest>(r =>
                        r.CaseId == cases[3].CaseId &&
                        r.HashedLegalEntityId == cases[3].ApprenticeshipLegalEntityId &&
                        r.Status == cases[3].CaseStatus &&
                        r.CaseStatusLastUpdatedDate == cases[3].CaseStatusLastUpdatedDate)), Times.Once);
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
    }
}
