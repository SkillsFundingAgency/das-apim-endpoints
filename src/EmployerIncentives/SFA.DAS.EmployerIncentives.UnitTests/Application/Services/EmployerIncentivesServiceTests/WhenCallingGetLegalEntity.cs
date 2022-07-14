using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Interfaces;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Services
{
    public class WhenCallingGetLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_LegalEntity(
            long accountId,
            long accountLegalEntityId,
            AccountLegalEntity apiResponse,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            LegalEntitiesService service
        )
        {
            client.Setup(x =>
                    x.Get<AccountLegalEntity>(
                        It.Is<GetLegalEntityRequest>(c => c.GetUrl.Contains(accountId.ToString()) && c.GetUrl.Contains(accountLegalEntityId.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetLegalEntity(accountId, accountLegalEntityId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}