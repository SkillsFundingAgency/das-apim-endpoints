using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingDeleteLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_To_Remove_The_LegalEntity_For_An_Account(
            long accountId,
            long accountLegalEntityId,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            LegalEntitiesService service)
        {

            await service.DeleteAccountLegalEntity(accountId, accountLegalEntityId);
            
            client.Verify(x =>
                    x.Delete(
                        It.Is<DeleteAccountLegalEntityRequest>(c => 
                            c.DeleteUrl.Contains(accountId.ToString())
                            && c.DeleteUrl.Contains(accountLegalEntityId.ToString()))), Times.Once);

        }
    }
}