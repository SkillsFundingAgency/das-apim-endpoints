using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ProviderPR.Application.Relationships.Queries.GetRelationships;
using SFA.DAS.ProviderPR.Infrastructure;
using SFA.DAS.ProviderPR.InnerApi.Responses;

namespace SFA.DAS.ProviderPR.UnitTests.Application.Queries.GetRelationships;
public class GetRelationshipQueryHandlerTests
{
    [Test, AutoData]
    public async Task Handler_InvokesApi_ReturnsResponse(GetRelationshipsQuery query, GetProviderRelationshipsResponse expected, CancellationToken cancellationToken)
    {
        Mock<IProviderRelationshipsApiRestClient> apiClientMock = new();
        Response<GetProviderRelationshipsResponse> result = new(null, new(HttpStatusCode.OK), () => expected);
        apiClientMock.Setup(c => c.GetProviderRelationships(query.Ukprn, query.Request.ToQueryString(), cancellationToken)).ReturnsAsync(result);

        GetRelationshipsQueryHandler sut = new(apiClientMock.Object);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected);
    }
}
