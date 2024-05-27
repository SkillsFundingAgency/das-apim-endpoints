using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.EmployerRelationships.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnEmployerRelaionships(
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> providerRelationshipsApiClient,
        GetEmployerRelationshipsQueryHandler handler,
        GetEmployerRelationshipsQuery query,
        GetEmployerRelationshipsResponse response
    )
    {
        providerRelationshipsApiClient.Setup(x =>
            x.Get<GetEmployerRelationshipsResponse>(
                It.Is<GetEmployerRelationshipsRequest>(x =>
                    x.AccountHashedId == query.AccountHashedId &&
                    x.Ukprn == query.Ukprn &&
                    x.AccountlegalentityPublicHashedId == query.AccountlegalentityPublicHashedId
                )
            )
        ).ReturnsAsync(response);

        var actual = await handler.Handle(query, CancellationToken.None);
        actual.AccountLegalEntities.Should().BeEquivalentTo(response.AccountLegalEntities.Select(acl => (AccountLegalEntityPermissionsModel)acl));
    }
}
