using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerPR.Application.Queries.GetRelationships;
using SFA.DAS.EmployerPR.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerPR.UnitTests.Application.Relationships.Queries.GetRelationships;
public class GetRelationshipsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnRelationships(
        [Frozen] Mock<IProviderRelationshipsApiRestClient> providerRelationshipsApiRestClient,
        GetRelationshipsHandler handler,
        GetRelationshipsQuery query,
        GetRelationshipsResponse response
    )
    {
        providerRelationshipsApiRestClient.Setup(x =>
            x.GetRelationships(
                query.Ukprn,
                query.AccountLegalEntityId,
                CancellationToken.None
            )
        ).ReturnsAsync(response);

        var actual = await handler.Handle(query, CancellationToken.None);
        actual.Should().BeEquivalentTo(response);
    }
}
