using System.Threading.Tasks;
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

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.CustomerEngagementFinanceServiceTests
{
    public class GetVendorRegistrationStatusByCaseId
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Registration_Status(
            string caseId,
            GetVendorRegistrationStatusByCaseIdResponse apiResponse,
            [Frozen] Mock<ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>> client,
            CustomerEngagementFinanceService service
        )
        {
            client.Setup(x =>
                    x.Get<GetVendorRegistrationStatusByCaseIdResponse>(
                        It.Is<GetVendorRegistrationStatusByCaseIdRequest>(c => c.GetUrl.Contains(caseId)), false))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetVendorRegistrationStatusByCaseId(caseId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}