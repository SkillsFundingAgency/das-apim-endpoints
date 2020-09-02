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
    public class WhenCallingGetVendorByApprenticeshipLegalEntityId
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Vendor(
            string companyName,
            string hashedLegalEntityId,
            GetVendorByApprenticeshipLegalEntityIdResponse apiResponse,
            [Frozen] Mock<ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration>> client,
            CustomerEngagementFinanceService service
        )
        {
            client.Setup(x =>
                    x.Get<GetVendorByApprenticeshipLegalEntityIdResponse>(
                        It.Is<GetVendorByApprenticeshipLegalEntityId>(c => c.GetUrl.Contains(companyName) && c.GetUrl.Contains(hashedLegalEntityId)), true))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetVendorByApprenticeshipLegalEntityId(companyName, hashedLegalEntityId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}