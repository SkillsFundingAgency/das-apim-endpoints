using System.Net;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;
using SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.StagedApprentices.Queries.GetStagedApprentice;

public class GetStagedApprenticeQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeResponse result,
        GetStagedApprenticeQuery query,
        CancellationToken cancellationToken)
    {
        Response<GetStagedApprenticeResponse?> apiResponse = new(string.Empty, new(HttpStatusCode.OK), () => result);
        apiClientMock.Setup(c => c.GetStagedApprentice(query.LastName, query.DateOfBirth, query.Email, cancellationToken)).ReturnsAsync(apiResponse);

        await sut.Handle(query, cancellationToken);

        apiClientMock.Verify(c => c.GetStagedApprentice(query.LastName, query.DateOfBirth, query.Email, cancellationToken));
    }

    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClientWithEncodedEmail(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeResponse result,
        CancellationToken cancellationToken)
    {
        var email = "john_s.smith+esfa@g-mail.com";
        var expectedEmail = HttpUtility.UrlEncode(email);
        GetStagedApprenticeQuery query = new(Guid.NewGuid().ToString(), DateTime.Now, email);

        Response<GetStagedApprenticeResponse?> apiResponse = new(string.Empty, new(HttpStatusCode.OK), () => result);
        apiClientMock.Setup(c => c.GetStagedApprentice(query.LastName, query.DateOfBirth, expectedEmail, cancellationToken)).ReturnsAsync(apiResponse);

        await sut.Handle(query, cancellationToken);

        apiClientMock.Verify((c => c.GetStagedApprentice(query.LastName, query.DateOfBirth, expectedEmail, cancellationToken)));
    }


    [Test, MoqAutoData]
    public async Task Handle_OnOkApiResponse_ReturnsData(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeResponse result,
        GetStagedApprenticeQuery query,
        CancellationToken cancellationToken)
    {
        Response<GetStagedApprenticeResponse?> apiResponse = new(string.Empty, new(HttpStatusCode.OK), () => result);
        apiClientMock.Setup(c => c.GetStagedApprentice(query.LastName, query.DateOfBirth, query.Email, cancellationToken)).ReturnsAsync(apiResponse);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(result);
    }

    [Test, MoqAutoData]
    public async Task Handle_OnNotFoundApiResponse_ReturnsNull(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeQuery query,
        CancellationToken cancellationToken)
    {
        Response<GetStagedApprenticeResponse?> apiResponse = new(string.Empty, new(HttpStatusCode.NotFound), () => null);
        apiClientMock.Setup(c => c.GetStagedApprentice(query.LastName, query.DateOfBirth, query.Email, cancellationToken)).ReturnsAsync(apiResponse);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_OnUnsuccessfulApiResponse_ThrowsException(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeQuery query,
        CancellationToken cancellationToken)
    {
        Response<GetStagedApprenticeResponse?> apiResponse = new(string.Empty, new(HttpStatusCode.BadRequest), () => null);
        apiClientMock.Setup(c => c.GetStagedApprentice(query.LastName, query.DateOfBirth, query.Email, cancellationToken)).ReturnsAsync(apiResponse);

        Func<Task> action = () => sut.Handle(query, cancellationToken);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
