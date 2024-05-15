using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using RestEase;
using SFA.DAS.RoatpProviderModeration.Application.Infrastructure;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Responses;
using SFA.DAS.RoatpProviderModeration.Application.Provider.Queries.GetProvider;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpProviderModeration.Application.UnitTests.Providers.Queries;

[TestFixture]
public class GetProviderQueryHandlerTests
{
    [Test, RecursiveMoqAutoData]
    public async Task Handle_CallsInnerApi_ReturnsValidResponse(
        GetProviderResponse apiResponseProvider,
        GetProviderQuery query,
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CancellationToken cancellationToken,
        GetProviderQueryHandler sut)
    {
        apiClientMock.Setup(c => c.GetProvider(query.Ukprn, cancellationToken))
            .ReturnsAsync(new Response<GetProviderResponse>(string.Empty, new(HttpStatusCode.OK), () => apiResponseProvider));
        var result = await sut.Handle(query, new CancellationToken());
        result.Should().NotBeNull();
        result.MarketingInfo.Should().BeEquivalentTo(apiResponseProvider.MarketingInfo);
        result.ProviderType.Should().Be(apiResponseProvider.ProviderType);
    }

    [Test, RecursiveMoqAutoData]
    public async Task Handle_CallsInnerApi_404ReturnsNull(
        GetProviderResponse apiResponseProvider,
        GetProviderQuery query,
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CancellationToken cancellationToken,
        GetProviderQueryHandler sut)
    {
        apiClientMock.Setup(c => c.GetProvider(query.Ukprn, cancellationToken))
            .ReturnsAsync(new Response<GetProviderResponse>(string.Empty, new(HttpStatusCode.NotFound), () => apiResponseProvider));

        var result = await sut.Handle(query, new CancellationToken());
        result.Should().BeNull();
    }

    [Test, RecursiveMoqAutoData]
    public void Handle_CallsInnerApi_ReturnsExceptionWhenProviderGetsBadRequest(
        GetProviderResponse apiResponseProvider,
        GetProviderQuery query,
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CancellationToken cancellationToken,
        GetProviderQueryHandler sut)
    {
        apiClientMock.Setup(c => c.GetProvider(query.Ukprn, cancellationToken))
            .ReturnsAsync(new Response<GetProviderResponse>(string.Empty, new(HttpStatusCode.BadRequest), () => apiResponseProvider));

        Assert.ThrowsAsync<InvalidOperationException>(() => sut.Handle(query, new CancellationToken()));
    }
}
