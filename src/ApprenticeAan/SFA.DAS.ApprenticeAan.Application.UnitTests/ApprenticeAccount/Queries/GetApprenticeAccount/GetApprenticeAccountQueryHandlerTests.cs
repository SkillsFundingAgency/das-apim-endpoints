using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.ApprenticeAccount.Queries.GetApprenticeAccount;

public class GetApprenticeAccountQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
    [Frozen] Mock<IApprenticeAccountsApiClient> apiClientMock,
    GetApprenticeAccountQueryHandler sut,
    Guid apprenticeId,
    GetApprenticeAccountQueryResult expectedResult,
    CancellationToken cancellationToken)
    {
        const HttpStatusCode status = HttpStatusCode.OK;

        var query = new GetApprenticeAccountQuery { ApprenticeId = apprenticeId };
        apiClientMock.Setup(c => c.GetApprentice(apprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(
                new RestEase.Response<GetApprenticeAccountQueryResult>(
                    "not used",
                    new HttpResponseMessage(status),
                    () => expectedResult)
            );

        await sut.Handle(query, cancellationToken);

        apiClientMock.Verify(c => c.GetApprentice(apprenticeId, It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Handle_ApprenticeFound_ReturnsApprentice(
        [Frozen] Mock<IApprenticeAccountsApiClient> apiClientMock,
        GetApprenticeAccountQueryHandler sut,
        Guid apprenticeId,
        GetApprenticeAccountQueryResult expectedResult,
        CancellationToken cancellationToken)
    {
        const HttpStatusCode status = HttpStatusCode.OK;

        GetApprenticeAccountQuery request = new() { ApprenticeId = apprenticeId };

        apiClientMock.Setup(c => c.GetApprentice(apprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(
            new RestEase.Response<GetApprenticeAccountQueryResult>(
                "not used",
                new HttpResponseMessage(status),
                () => expectedResult)
        );

        var actualResult = await sut.Handle(request, cancellationToken);

        Assert.That(expectedResult, Is.EqualTo(actualResult));
    }

    [Test, MoqAutoData]
    public async Task Handle_ApprenticeNotFound_ReturnsNullResult(
        [Frozen] Mock<IApprenticeAccountsApiClient> apiClientMock,
        GetApprenticeAccountQueryHandler sut,
        Guid apprenticeId,
        CancellationToken cancellationToken)
    {

        const HttpStatusCode status = HttpStatusCode.NotFound;

        GetApprenticeAccountQuery request = new() { ApprenticeId = apprenticeId };

        apiClientMock.Setup(c => c.GetApprentice(apprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(
            new RestEase.Response<GetApprenticeAccountQueryResult>(
                "not used",
                new HttpResponseMessage(status),
                () => null!)
        );

        var actualResult = await sut.Handle(request, cancellationToken);

        Assert.That(actualResult, Is.Null);
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ApiInvalidResponse_ThrowsInvalidOperationException(
        [Frozen] Mock<IApprenticeAccountsApiClient> apiClientMock,
        GetApprenticeAccountQueryHandler sut,
        GetApprenticeAccountQuery request,
        CancellationToken cancellationToken)
    {
        const HttpStatusCode status = HttpStatusCode.InternalServerError;

        apiClientMock.Setup(c => c.GetApprentice(request.ApprenticeId, It.IsAny<CancellationToken>())).ReturnsAsync(
            new RestEase.Response<GetApprenticeAccountQueryResult>(
            "not used",
            new HttpResponseMessage(status),
            () => null!)
            );

        Func<Task> act = () => sut.Handle(request, cancellationToken);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
