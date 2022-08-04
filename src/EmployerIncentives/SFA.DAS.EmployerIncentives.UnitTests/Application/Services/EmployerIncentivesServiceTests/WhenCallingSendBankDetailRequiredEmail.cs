using NUnit.Framework;
using System.Threading.Tasks;
using SFA.DAS.Testing.AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingSendBankDetailRequiredEmail
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
           PostBankDetailsRequiredEmailRequest requestData,
           [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
           EmailService sut)
        {
            await sut.SendEmail<PostBankDetailsRequiredEmailRequest>(requestData);

            client.Verify(x =>
                x.PostWithResponseCode<PostBankDetailsRequiredEmailRequest>(It.Is<PostBankDetailsRequiredEmailRequest>(
                    c => c.Data == requestData.Data &&
                         c.PostUrl.Contains("bank-details-required")
                ), false), Times.Once);
        }
    }
}
