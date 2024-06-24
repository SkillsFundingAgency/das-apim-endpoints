using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.EmployerRelationships.Queries.GetEmployerRelationships;

public class GetEmployerRelationshipsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnEmployerRelaionships(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        GetEmployerRelationshipsQueryHandler handler,
        GetEmployerRelationshipsQuery query,
        GetEmployerRelationshipsResponse response
    )
    {
        providerRelationshipsApiRestClient.Setup(x =>
            x.GetEmployerRelationships(
                query.AccountHashedId, 
                query.Ukprn, 
                query.AccountlegalentityPublicHashedId,
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var actual = await handler.Handle(query, CancellationToken.None);
        actual.AccountLegalEntities.Should().BeEquivalentTo(response.AccountLegalEntities.Select(acl => (AccountLegalEntityPermissionsModel)acl));
    }
}
