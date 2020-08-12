using NUnit.Framework;
using System;
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
           SendBankDetailsEmailRequest requestData,
           long accountId,
           [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
           EmployerIncentivesService sut)
        {
            await sut.SendBankDetailRequiredEmail(accountId, requestData);

            client.Verify(x =>
                x.Post<SendBankDetailsEmailRequest>(It.Is<PostSendBankDetailsEmailRequest>(
                    c => c.Data == requestData &&
                         c.PostUrl.Contains("bank-details-required")
                )), Times.Once);
        }
    }
}
