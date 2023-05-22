using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.EmployerAan.Application.Employer.Queries.GetEmployerMember;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Employer.Queries.GetEmployerMember;
public class GetEmployerMemberQueryHandlerTests
{
    [Test]
    [AutoData]
    public async Task Handle_OkApiResponse_ReturnsResult(
        GetEmployerMemberQuery query,
        GetEmployerMemberQueryResult expectedResponse,
        CancellationToken cancellationToken)
    {
        Mock<IAanHubRestApiClient> apiClientMock = new();
        apiClientMock
            .Setup(c => c.GetEmployer(query.UserRef, cancellationToken))
            .ReturnsAsync(new Response<GetEmployerMemberQueryResult?>(string.Empty, new(HttpStatusCode.OK), () => expectedResponse));
        GetEmployerMemberQueryHandler sut = new(apiClientMock.Object);
        var result = await sut.Handle(query, cancellationToken);
        result.Should().Be(expectedResponse);
    }

    [Test]
    [AutoData]
    public async Task Handle_NotFoundApiResponse_ReturnsNull(
        GetEmployerMemberQuery query,
        CancellationToken cancellationToken)
    {
        Mock<IAanHubRestApiClient> apiClientMock = new();
        apiClientMock
            .Setup(c => c.GetEmployer(query.UserRef, cancellationToken))
            .ReturnsAsync(new Response<GetEmployerMemberQueryResult?>(string.Empty, new(HttpStatusCode.NotFound), () => null));
        GetEmployerMemberQueryHandler sut = new(apiClientMock.Object);
        var result = await sut.Handle(query, cancellationToken);
        result.Should().BeNull();
    }


    [Test]
    [AutoData]
    public async Task Handle_UnexpectedApiResponse_ThrowsException(
        GetEmployerMemberQuery query,
        CancellationToken cancellationToken)
    {
        Mock<IAanHubRestApiClient> apiClientMock = new();
        apiClientMock
            .Setup(c => c.GetEmployer(query.UserRef, cancellationToken))
            .ReturnsAsync(new Response<GetEmployerMemberQueryResult?>(string.Empty, new(HttpStatusCode.InternalServerError), () => null));
        GetEmployerMemberQueryHandler sut = new(apiClientMock.Object);

        Func<Task> action = () => sut.Handle(query, cancellationToken);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
