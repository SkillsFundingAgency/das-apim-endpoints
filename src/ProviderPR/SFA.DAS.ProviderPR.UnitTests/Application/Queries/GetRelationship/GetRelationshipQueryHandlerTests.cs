using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ProviderPR.Application.Queries.GetRelationship;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Queries.GetRelationship;

public class GetRelationshipQueryHandlerTests
{
    [Test, AutoData]
    public async Task Handler_InvokesApi_ReturnsResponse(GetRelationshipQuery query, GetRelationshipResponse expected, CancellationToken cancellationToken)
    {
        Mock<IProviderRelationshipsApiRestClient> apiClientMock = new();
        Response<GetRelationshipResponse> result = new(null, new(HttpStatusCode.OK), () => expected);
        apiClientMock.Setup(c => c.GetRelationship(query.Ukprn, query.AccountLegalEntityId, cancellationToken)).ReturnsAsync(result);

        GetRelationshipQueryHandler sut = new(apiClientMock.Object);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }
}
