using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;
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
        string hashedAccountId,
        List<GetAccountLegalEntityResponse> expected)
    {
        var request = new GetAccountLegalEntitiesRequest(hashedAccountId);
        var query = new GetAccountLegalEntitiesQuery { HashedAccountId = hashedAccountId };

        accountsApiClient.Setup(x =>
            x.GetAll<GetAccountLegalEntityResponse>(request)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, new CancellationToken());
        actual.Should().Be(expected?.Select(legalEntities => (LegalEntity)legalEntities));
    }
}
