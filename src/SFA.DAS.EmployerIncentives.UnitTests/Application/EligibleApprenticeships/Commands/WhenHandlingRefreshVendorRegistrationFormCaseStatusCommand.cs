using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Commands.AddEmployerVendorId;
using SFA.DAS.EmployerIncentives.Application.Commands.UpdateVendorRegistrationFormCaseStatus;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
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
            GetVendorRegistrationCaseStatusUpdateResponse vendorResponse)
        {
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(DateTime.Now.AddHours(-1));

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1)))
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
        public async Task Then_the_Vrf_case_status_is_updated_once_for_each_legal_entity_using_the_latest_case_details_and_next_update_date_is_returned(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler)
        {

            var fixture = new Fixture();

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

            var command = new RefreshVendorRegistrationFormCaseStatusCommand(DateTime.Now.AddHours(-1));

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1)))
                .ReturnsAsync(financeApiResponse);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));

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
        public async Task And_no_cases_returned_from_FinanceAPI_Then_The_legal_entities_are_not_updated_and_current_UTC_DateTime_is_returned__when_to_date_is_in_future(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler)
        {
            var fromDateTime = DateTime.UtcNow.AddHours(-1);
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(fromDateTime);

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1)))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse());

            var result = await handler.Handle(command, CancellationToken.None);

            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Never());

            result.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        }


        [Test, MoqAutoData]
        public async Task And_no_cases_returned_from_FinanceAPI_Then_The_legal_entities_are_not_updated_and_from_date_plus_1_day_is_returned__when_ToDate_is_in_past(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler)
        {
            var fromDateTime = DateTime.UtcNow.AddHours(-25);
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(fromDateTime);

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1)))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse());

            var result = await handler.Handle(command, CancellationToken.None);

            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Never());

            result.Should().Be(fromDateTime.AddDays(1));
        }

        [Test, MoqAutoData]
        public void And_null_response_returned_from_FinanceAPI_Then_The_legal_entities_are_not_updated_and_exception_is_thrown(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            DateTime fromDateTime,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler)
        {
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(fromDateTime);

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1)))
                .ReturnsAsync(new GetVendorRegistrationCaseStatusUpdateResponse { RegistrationCases = null });

            Func<Task> f = async () => await handler.Handle(command, CancellationToken.None);

            f.Should().Throw<ArgumentNullException>();

            incentivesService.Verify(
                x => x.UpdateVendorRegistrationCaseStatus(It.IsAny<UpdateVendorRegistrationCaseStatusRequest>()), Times.Never());
        }

        [Test, MoqAutoData]
        public async Task Then_The_vendor_ids_are_updated_with_the_vendor_registration_form_Details_if_not_already_set(
            [Frozen] Mock<ICustomerEngagementFinanceService> financeService,
            [Frozen] Mock<IEmployerIncentivesService> incentivesService,
            [Frozen] Mock<IMediator> mediator,
            RefreshVendorRegistrationFormCaseStatusCommandHandler handler,
            GetVendorRegistrationCaseStatusUpdateResponse vendorResponse)
        {
            var command = new RefreshVendorRegistrationFormCaseStatusCommand(DateTime.Now.AddHours(-1));

            financeService.Setup(x => x.GetVendorRegistrationCasesByLastStatusChangeDate(command.FromDateTime, command.FromDateTime.AddDays(1)))
                .ReturnsAsync(vendorResponse);

            var response = new AccountLegalEntity { VrfVendorId = string.Empty };
            incentivesService.Setup(x => x.GetLegalEntityByHashedId(It.IsAny<string>())).ReturnsAsync(response);

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
                incentivesService.Verify(x => x.GetLegalEntityByHashedId(It.Is<string>(r => r == @case.ApprenticeshipLegalEntityId)), Times.Once);

                mediator.Verify(x => x.Send(It.Is<GetAndAddEmployerVendorIdCommand>(r => r.HashedLegalEntityId == @case.ApprenticeshipLegalEntityId), It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}
