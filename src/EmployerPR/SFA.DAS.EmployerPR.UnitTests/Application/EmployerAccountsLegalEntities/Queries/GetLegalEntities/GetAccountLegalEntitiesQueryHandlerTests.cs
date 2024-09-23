using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.AccountLegalEntities.Queries.GetAccountLegalEntities;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.EmployerAccountsLegalEntities.Queries.GetLegalEntities;
public class GetAccountLegalEntitiesQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetAccountLegalEntitiesQueryHandler handler,
        long accountId,
        List<GetAccountLegalEntityResponse> expected)
    {
        var query = new GetAccountLegalEntitiesQuery { AccountId = accountId };

        accountsApiClient.Setup(x =>
            x.GetAll<GetAccountLegalEntityResponse>(It.IsAny<GetAccountLegalEntitiesRequest>())).ReturnsAsync(expected);
        var actual = await handler.Handle(query, new CancellationToken());
        actual.LegalEntities.Should().BeEquivalentTo(expected?.Select(legalEntities => (LegalEntity)legalEntities));
    }
}
