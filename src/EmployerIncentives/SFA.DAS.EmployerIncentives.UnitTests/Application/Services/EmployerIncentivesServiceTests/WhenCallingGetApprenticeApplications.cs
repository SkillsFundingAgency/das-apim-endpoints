using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    [TestFixture]
    public class WhenCallingGetApprenticeApplications
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_Applications_For_The_Account(
            long accountId,
            long accountLegalEntityId,
            PaymentApplicationsDto api,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            ApplicationService service
        )
        {
            client.Setup(x =>
                    x.Get<PaymentApplicationsDto>(
                        It.Is<GetApplicationsRequest>(c => c.GetUrl.Contains(accountId.ToString()))))
                .ReturnsAsync(api);

            var actual = await service.GetPaymentApplications(accountId, accountLegalEntityId);

            actual.Should().BeEquivalentTo(api);
        }
    }
}
