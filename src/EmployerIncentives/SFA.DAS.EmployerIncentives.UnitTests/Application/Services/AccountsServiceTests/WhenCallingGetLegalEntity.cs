using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Application.Services;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Accounts;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.UnitTests.Application.Services.AccountsServiceTests
{
    public class WhenCallingGetLegalEntity
    {
        [Test, MoqAutoData]
        public async Task Then_The_Api_Is_Called_Returning_The_Legal_Entity(
            string accountId,
            long legalEntityId,
            LegalEntity apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> client,
            AccountsService service
        )
        {
            client.Setup(x =>
                    x.Get<LegalEntity>(
                        It.Is<GetLegalEntityRequest>(c => c.GetUrl.Contains(accountId) && c.GetUrl.Contains(legalEntityId.ToString()))))
                .ReturnsAsync(apiResponse);

            var actual = await service.GetLegalEntity(accountId, legalEntityId);

            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}