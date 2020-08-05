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
    public class WhenCallingPostLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_To_Create_And_The_New_LegalEntity_Is_Returned(
            long accountId,
            AccountLegalEntityCreateRequest createObject,
            AccountLegalEntity clientResponse,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service)
        {
            client.Setup(x => 
                x.Post<AccountLegalEntity>(It.Is<PostAccountLegalEntityRequest>(
                    c=>
                        c.PostUrl.Contains(accountId.ToString())
                        && c.Data.IsSameOrEqualTo(createObject)
                        ))
                ).ReturnsAsync(clientResponse);
            
            var actual = await service.CreateLegalEntity(accountId, createObject);

            actual.Should().BeEquivalentTo(clientResponse);
        }
    }
}