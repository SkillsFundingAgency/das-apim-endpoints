using System.Net;
using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.ApprenticeAan.Application.StagedApprentices.Queries.GetStagedApprentice;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.StagedApprentices.Queries.GetStagedApprentice;

public class GetStagedApprenticeQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeQueryResult result,
        GetStagedApprenticeQuery query)
    {
        ApiResponse<GetStagedApprenticeQueryResult?> apiResponse = new(result, HttpStatusCode.OK, null);
        apiClientMock.Setup(c => c.GetWithResponseCode<GetStagedApprenticeQueryResult?>(It.Is<GetStagedApprenticeRequest>(r => r.LastName == query.LastName && r.DateOfBirth == query.DateOfBirth && r.Email == query.Email))).ReturnsAsync(apiResponse);

        await sut.Handle(query, new CancellationToken());

        apiClientMock.Verify(c => c.GetWithResponseCode<GetStagedApprenticeQueryResult?>(It.Is<GetStagedApprenticeRequest>(r => r.LastName == query.LastName && r.DateOfBirth == query.DateOfBirth && r.Email == query.Email)));
    }

    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClientWithEncodedEmail(
    [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock,
    GetStagedApprenticeQueryHandler sut,
    GetStagedApprenticeQueryResult result)
    {
        var email = "john_s.smith+esfa@g-mail.com";
        var expectedEmail = HttpUtility.UrlEncode(email);
        GetStagedApprenticeQuery query = new(Guid.NewGuid().ToString(), DateTime.Now, email);

        ApiResponse<GetStagedApprenticeQueryResult?> apiResponse = new(result, HttpStatusCode.OK, null);
        apiClientMock.Setup(c => c.GetWithResponseCode<GetStagedApprenticeQueryResult?>(It.Is<GetStagedApprenticeRequest>(r => r.LastName == query.LastName && r.DateOfBirth == query.DateOfBirth && r.Email == expectedEmail))).ReturnsAsync(apiResponse);

        await sut.Handle(query, new CancellationToken());

        apiClientMock.Verify(c => c.GetWithResponseCode<GetStagedApprenticeQueryResult?>(It.Is<GetStagedApprenticeRequest>(r => r.Email == expectedEmail)));
    }


    [Test, MoqAutoData]
    public async Task Handle_OnOkApiResponse_ReturnsData(
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeQueryResult expected,
        GetStagedApprenticeQuery query)
    {
        ApiResponse<GetStagedApprenticeQueryResult?> apiResponse = new(expected, HttpStatusCode.OK, null);
        apiClientMock.Setup(c => c.GetWithResponseCode<GetStagedApprenticeQueryResult?>(It.Is<GetStagedApprenticeRequest>(r => r.LastName == query.LastName && r.DateOfBirth == query.DateOfBirth && r.Email == query.Email))).ReturnsAsync(apiResponse);

        var actual = await sut.Handle(query, new CancellationToken());

        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_OnNotFoundApiResponse_ReturnsNull(
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeQuery query)
    {
        ApiResponse<GetStagedApprenticeQueryResult?> apiResponse = new(null, HttpStatusCode.NotFound, null);
        apiClientMock.Setup(c => c.GetWithResponseCode<GetStagedApprenticeQueryResult?>(It.IsAny<GetStagedApprenticeRequest>())).ReturnsAsync(apiResponse);

        var actual = await sut.Handle(query, new CancellationToken());

        actual.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_OnUnsuccessfulApiResponse_ThrowsException(
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock,
        GetStagedApprenticeQueryHandler sut,
        GetStagedApprenticeQuery query)
    {
        ApiResponse<GetStagedApprenticeQueryResult?> apiResponse = new(null, HttpStatusCode.BadRequest, null);
        apiClientMock.Setup(c => c.GetWithResponseCode<GetStagedApprenticeQueryResult?>(It.IsAny<GetStagedApprenticeRequest>())).ReturnsAsync(apiResponse);

        Func<Task> action = () => sut.Handle(query, new CancellationToken());

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
