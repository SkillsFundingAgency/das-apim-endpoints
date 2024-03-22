using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RestEase;
using SFA.DAS.Roatp.Application.Charities.Queries;
using SFA.DAS.Roatp.Domain.Models;
using SFA.DAS.Roatp.Infrastructure;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Roatp.UnitTests.GetCharities.Queries.GetCharityByReferenceNumber;
public class GetCharityQueryHandlerTests
{
    [Test, AutoData]
    public async Task Handle_OkApiResponse_ReturnsResult(
        GetCharityQuery query,
        GetCharityResponse expectedResponse,
        CancellationToken cancellationToken)
    {
        Mock<ICharitiesRestApiClient> apiClientMock = new();
        apiClientMock
            .Setup(c => c.GetCharities(query.RegistrationNumber, cancellationToken))
            .ReturnsAsync(new Response<GetCharityResponse?>(string.Empty, new(HttpStatusCode.OK), () => expectedResponse));
        GetCharityQueryHandler sut = new(apiClientMock.Object, Mock.Of<ILogger<GetCharityQueryHandler>>());
        var result = await sut.Handle(query, cancellationToken);
        result.Charity.Should().BeEquivalentTo((Charity)expectedResponse);
    }

    [Test, AutoData]
    public async Task Handle_NotFoundApiResponse_ReturnsNull(
        GetCharityQuery query,
        CancellationToken cancellationToken)
    {
        Mock<ICharitiesRestApiClient> apiClientMock = new();
        apiClientMock
            .Setup(c => c.GetCharities(query.RegistrationNumber, cancellationToken))
            .ReturnsAsync(new Response<GetCharityResponse?>(string.Empty, new(HttpStatusCode.NotFound), () => null));
        GetCharityQueryHandler sut = new(apiClientMock.Object, Mock.Of<ILogger<GetCharityQueryHandler>>());
        var result = await sut.Handle(query, cancellationToken);
        result.Should().BeNull();
    }


    [Test, AutoData]
    public async Task Handle_UnexpectedApiResponse_ThrowsException(
        GetCharityQuery query,
        CancellationToken cancellationToken)
    {
        Mock<ICharitiesRestApiClient> apiClientMock = new();
        apiClientMock
            .Setup(c => c.GetCharities(query.RegistrationNumber, cancellationToken))
            .ReturnsAsync(new Response<GetCharityResponse?>(string.Empty, new(HttpStatusCode.InternalServerError), () => null));
        GetCharityQueryHandler sut = new(apiClientMock.Object, Mock.Of<ILogger<GetCharityQueryHandler>>());

        Func<Task> action = () => sut.Handle(query, cancellationToken);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
