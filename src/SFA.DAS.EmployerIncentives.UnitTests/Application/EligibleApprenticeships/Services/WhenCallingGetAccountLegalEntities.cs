using System.Threading.Tasks;
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

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.EligibleApprenticeships.Services
{
    public class WhenCallingGetAccountLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_LegalEntities_For_The_Account(
            long accountId,
            GetAccountLegalEntitiesResponse apiResponse,
            [Frozen] Mock<IEmployerIncentivesApiClient<EmployerIncentivesConfiguration>> client,
            EmployerIncentivesService service
        )
        {
            client.Setup(x =>
                    x.Get<GetAccountLegalEntitiesResponse>(
                        It.Is<GetAccountLegalEntitiesRequest>(c => c.GetUrl.Contains(accountId.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetAccountLegalEntities(accountId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}