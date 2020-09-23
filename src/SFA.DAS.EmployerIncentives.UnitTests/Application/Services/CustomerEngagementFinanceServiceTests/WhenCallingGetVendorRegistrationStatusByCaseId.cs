using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.VendorRegistrationForm;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.CustomerEngagementFinanceServiceTests
{
    public class GetVendorRegistrationStatusByCaseId
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_with_passed_in_to_and_from_datetime(
            string caseId,
            GetVendorRegistrationCaseStatusUpdateResponse apiResponse,
            [Frozen] Mock<ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>> client,
            CustomerEngagementFinanceService service,
            DateTime fromDateTime,
            DateTime toDateTime
        )
        {
            var expectedUrl = $"Finance/Registrations?DateTimeFrom={fromDateTime:yyyyMMddHHmmss}&DateTimeTo={toDateTime:yyyyMMddHHmmss}&VendorType=EMPLOYER&api-version=2019-06-01";
            client.Setup(x =>
                    x.Get<GetVendorRegistrationCaseStatusUpdateResponse>(
                        It.Is<GetVendorRegistrationStatusByLastStatusChangeDateRequest>(c =>
                            c.GetUrl.Contains(expectedUrl)), false))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetVendorRegistrationCasesByLastStatusChangeDate(fromDateTime, toDateTime);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}