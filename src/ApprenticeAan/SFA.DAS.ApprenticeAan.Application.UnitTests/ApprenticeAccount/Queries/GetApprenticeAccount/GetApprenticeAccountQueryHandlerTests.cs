using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.ApprenticeAccount.Queries.GetApprenticeAccount;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.ApprenticeAccount.Queries.GetApprenticeAccount;

public class GetApprenticeAccountQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_InvokesApiClient(
    [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
    GetApprenticeAccountQueryHandler sut,
    GetApprenticeAccountQuery request,
    GetApprenticeAccountQueryResult expectedResult,
    CancellationToken cancellationToken)
    {

        apiClientMock.Setup(c => c.GetWithResponseCode<GetApprenticeAccountQueryResult>(It.Is<GetApprenticeRequest>(r => r.Id == request.ApprenticeId))).ReturnsAsync(new ApiResponse<GetApprenticeAccountQueryResult>(expectedResult, HttpStatusCode.OK, null));

        await sut.Handle(request, cancellationToken);

        apiClientMock.Verify(c => c.GetWithResponseCode<GetApprenticeAccountQueryResult>(It.Is<GetApprenticeRequest>(r => r.Id == request.ApprenticeId)));
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ApprenticeFound_ReturnsApprentice(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
        GetApprenticeAccountQueryHandler sut,
        GetApprenticeAccountQuery request,
        GetApprenticeAccountQueryResult expectedResult,
        CancellationToken cancellationToken)
    {

        apiClientMock.Setup(c => c.GetWithResponseCode<GetApprenticeAccountQueryResult>(It.Is<GetApprenticeRequest>(r => r.Id == request.ApprenticeId))).ReturnsAsync(new ApiResponse<GetApprenticeAccountQueryResult>(expectedResult, HttpStatusCode.OK, null));

        var actualResult = await sut.Handle(request, cancellationToken);

        Assert.That(expectedResult, Is.EqualTo(actualResult));
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ApprenticeNotFound_ReturnsApprentice(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
        GetApprenticeAccountQueryHandler sut,
        GetApprenticeAccountQuery request,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetApprenticeAccountQueryResult?>(It.Is<GetApprenticeRequest>(r => r.Id == request.ApprenticeId))).ReturnsAsync(new ApiResponse<GetApprenticeAccountQueryResult?>(null, HttpStatusCode.NotFound, null));

        var actualResult = await sut.Handle(request, cancellationToken);

        Assert.That(actualResult, Is.Null);
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ApiInvalidResponse_ThrowsInvalidOperationException(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
        GetApprenticeAccountQueryHandler sut,
        GetApprenticeAccountQuery request,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.GetWithResponseCode<GetApprenticeAccountQueryResult?>(It.Is<GetApprenticeRequest>(r => r.Id == request.ApprenticeId))).ReturnsAsync(new ApiResponse<GetApprenticeAccountQueryResult?>(null, HttpStatusCode.InternalServerError, null));

        Func<Task> act = () => sut.Handle(request, cancellationToken);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
