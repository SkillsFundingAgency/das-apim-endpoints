using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Common;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingPutLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_To_Create_The_New_LegalEntity(
            AccountLegalEntityCreateRequest createObject,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            LegalEntitiesService service)
        {
            client.Setup(x => x.Put(It.Is<PutAccountLegalEntityRequest>(y =>
                y.PutUrl == $"accounts/{createObject.AccountId}/legalentities" && y.Data.Equals(createObject)))).Returns(Task.CompletedTask);
            
            await service.CreateLegalEntity(createObject);
        
            client.Verify(x => x.Put(It.Is<PutAccountLegalEntityRequest>(y =>
                y.PutUrl == $"accounts/{createObject.AccountId}/legalentities")), Times.Once);
        }
    }
}