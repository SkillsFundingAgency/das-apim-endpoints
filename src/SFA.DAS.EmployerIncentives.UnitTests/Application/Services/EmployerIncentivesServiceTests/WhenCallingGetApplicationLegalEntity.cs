using NUnit.Framework;
using System;
using System.Threading.Tasks;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;
using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.Application.Services;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingGetApplicationLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_The_InnerApi_Is_Called(
               long accountId,
               Guid applicationId,
               [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
               ApplicationService sut)
        {
            await sut.GetApplicationLegalEntity(accountId, applicationId);

            client.Verify(x =>
                x.Get<long>(It.Is<GetApplicationLegalEntityRequest>(
                    c => 
                        c.GetUrl.Contains("accountlegalentity")
                )), Times.Once);
        }
    }
}
