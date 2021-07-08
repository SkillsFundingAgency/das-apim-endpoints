using System.Collections.Generic;
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

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.EmployerIncentivesServiceTests
{
    public class WhenCallingGetAccountLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_LegalEntities_For_The_Account(
            long accountId,
            IEnumerable<AccountLegalEntity> apiResponse,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            LegalEntitiesService service
        )
        {
            client.Setup(x =>
                    x.GetAll<AccountLegalEntity>(
                        It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Contains(accountId.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetAccountLegalEntities(accountId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}