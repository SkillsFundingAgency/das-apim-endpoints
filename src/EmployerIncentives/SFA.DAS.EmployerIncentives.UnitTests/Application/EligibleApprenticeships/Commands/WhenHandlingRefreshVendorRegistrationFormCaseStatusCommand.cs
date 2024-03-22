using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Commands
{
    public class WhenHandlingRefreshVendorRegistrationFormCaseStatusCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_legal_entities_are_updated_with_the_vendor_registration_form_Details(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IVendorRegistrationService> vendorRegistrationService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            GetVendorRegistrationCaseStatusUpdateResponse vendorResponse)
        {
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(DateTime.Now.AddHours(-1));

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1), null))
                .ReturnsAsync(vendorResponse);

            await handler.Handle(command, CancellationToken.None);

            foreach (var @case in vendorResponse.RegistrationCases.Where(x => x.CaseStatus == "NEW"))
            {
                vendorRegistrationService.Verify(
                    x => x.UpdateVendorRegistrationCaseStatus(It.Is<UpdateVendorRegistrationCaseStatusRequest>(r =>
                            r.CaseId == @case.CaseId &&
                            r.HashedLegalEntityId == @case.ApprenticeshipLegalEntityId &&
                            r.Status == @case.CaseStatus &&
                            r.CaseStatusLastUpdatedDate.ToString() == @case.CaseStatusLastUpdatedDate)),
                    Times.Once());
            }
        }


        [Test, MoqAutoData]
        public async Task And_no_cases_returned_from_FinanceAPI_Then_The_legal_entities_are_not_updated_and_current_UTC_DateTime_is_returned__when_to_date_is_in_future(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IVendorRegistrationService> vendorRegistrationService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler)
        {
            var fromDateTime = DateTime.UtcNow.AddHours(-1);
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(fromDateTime);

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1), null))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse());

            var result = await handler.Handle(command, CancellationToken.None);

            vendorRegistrationService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Never());

            result.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }


        [Test, MoqAutoData]
        public async Task And_no_cases_returned_from_FinanceAPI_Then_The_legal_entities_are_not_updated_and_from_date_plus_1_day_is_returned__when_ToDate_is_in_past(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IVendorRegistrationService> vendorRegistrationService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler)
        {
            var fromDateTime = DateTime.UtcNow.AddHours(-25);
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(fromDateTime);

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1), null))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse());

            var result = await handler.Handle(command, CancellationToken.None);

            vendorRegistrationService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Never());

            result.Should().Be(fromDateTime.AddDays(1));
        }

        [Test, MoqAutoData]
        public void And_null_response_returned_from_FinanceAPI_Then_The_legal_entities_are_not_updated_and_exception_is_thrown(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IVendorRegistrationService> vendorRegistrationService,
            DateTime fromDateTime,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler)
        {
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(fromDateTime);

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1), null))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse { RegistrationCases = null });

            Func<Task> f = async () => await handler.Handle(command, CancellationToken.None);

            f.Should().ThrowAsync<ArgumentNullException>();

            vendorRegistrationService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Never());
        }

        [Test, MoqAutoData]
        public async Task And_response_with_SkipCode_returned_from_FinanceAPI_Then_a_call_is_made_using_skiptoken_parameter(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            GetLatestVendorRegistrationCaseUpdateDateTimeResponse lastUpdateResponse,
            string skipCode)
        {
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(lastUpdateResponse.LastUpdateDateTime.Value);

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value,
                    lastUpdateResponse.LastUpdateDateTime.Value.AddDays(1), null))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse { SkipCode = skipCode });
            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value,
                    lastUpdateResponse.LastUpdateDateTime.Value.AddDays(1), skipCode))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse());

            await handler.Handle(command, CancellationToken.None);

            financeService.Verify(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value,
                lastUpdateResponse.LastUpdateDateTime.Value.AddDays(1), null), Times.Once);
            financeService.Verify(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(lastUpdateResponse.LastUpdateDateTime.Value,
                lastUpdateResponse.LastUpdateDateTime.Value.AddDays(1), skipCode), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Then_paging_limit_is_enforced(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            RefreshVendorRegistrationFormCaseStatusCommand command,
            string skipCode)
        {

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse { SkipCode = skipCode });

            await handler.Handle(command, CancellationToken.None);

            financeService.Verify(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<string>()), Times.Exactly(5));
        }

        [Test, MoqAutoData]
        public async Task Then_case_status_last_updated_date_defaults_to_current_date_time_if_invalid_response_received_from_finance_API(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IVendorRegistrationService> vendorRegistrationService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler)
        {
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(DateTime.Now.AddHours(-1));
            var fixture = new Fixture();
            var vendorResponse = new GetVendorRegistrationCaseStatusUpdateResponse
            {
                RegistrationCases = new List<VendorRegistrationCase>
                {
                    new VendorRegistrationCase 
                    { 
                        CaseStatusLastUpdatedDate = "12/12/2022 12;34", 
                        ApprenticeshipLegalEntityId = fixture.Create<string>(),
                        CaseId = fixture.Create<string>(),
                        CaseStatus = fixture.Create<string>(),
                        CaseType = "NEW"
                    }
                }
            };

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1), null))
                .ReturnsAsync(vendorResponse);

            await handler.Handle(command, CancellationToken.None);

            vendorRegistrationService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.Is<UpdateVendorRegistrationCaseStatusRequest>(r =>
                    r.CaseId == vendorResponse.RegistrationCases[0].CaseId &&
                    r.HashedLegalEntityId == vendorResponse.RegistrationCases[0].ApprenticeshipLegalEntityId &&
                    r.Status == vendorResponse.RegistrationCases[0].CaseStatus &&
                    r.CaseStatusLastUpdatedDate.Date == DateTime.Today.Date)),
                Times.Once());
        }
    }
}
