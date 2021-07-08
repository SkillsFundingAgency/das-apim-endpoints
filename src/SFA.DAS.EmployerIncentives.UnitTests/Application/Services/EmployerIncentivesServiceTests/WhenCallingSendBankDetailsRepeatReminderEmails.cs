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
    public class WhenCallingSendBankDetailsRepeatReminderEmails
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
           SendBankDetailsRepeatReminderEmailsRequest requestData,
           long accountId,
           [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
           EmailService sut)
        {
            await sut.TriggerBankRepeatReminderEmails(requestData);

            client.Verify(x =>
                x.Post<SendBankDetailsRepeatReminderEmailsRequest>(It.Is<PostBankDetailsRepeatReminderEmailsRequest>(
                    c => c.Data == requestData &&
                         c.PostUrl.Contains("bank-details-repeat-reminders")
                )), Times.Once);
        }
    }
}
