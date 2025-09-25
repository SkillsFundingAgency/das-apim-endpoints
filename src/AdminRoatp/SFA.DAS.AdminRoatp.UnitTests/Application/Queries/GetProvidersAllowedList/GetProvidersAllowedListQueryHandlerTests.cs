using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedList;
using SFA.DAS.AdminRoatp.InnerApi.Requests.Roatp;
using SFA.DAS.AdminRoatp.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetProvidersAllowedList;
public class GetProvidersAllowedListQueryHandlerTests
{
    [Test, MoqAutoData]

    public async Task Handle_SuccessfulResponse_ReturnsData(
        [Frozen] Mock<IApplyApiClient<ApplyApiConfiguration>> apiClientMock,
        GetProvidersAllowedListQuery query,
        GetProvidersAllowedListQueryHandler sut,
        List<AllowedProvider> apiResponse)
    {
        var expectedResponse = new GetProvidersAllowedListQueryResponse { Providers = apiResponse };
        apiClientMock.Setup(a => a.GetWithResponseCode<List<AllowedProvider>>(It.Is<GetAllowedProvidersListRequest>(c => c.GetUrl.Equals(new GetAllowedProvidersListRequest(query.sortColumn, query.sortOrder).GetUrl)))).ReturnsAsync(new ApiResponse<List<AllowedProvider>>(apiResponse, HttpStatusCode.OK, ""));

        var result = await sut.Handle(query, CancellationToken.None);

        apiClientMock.Verify(a => a.GetWithResponseCode<List<AllowedProvider>>(It.Is<GetAllowedProvidersListRequest>(c => c.GetUrl.Equals(new GetAllowedProvidersListRequest(query.sortColumn, query.sortOrder).GetUrl))), Times.Once());
        result.Should().BeEquivalentTo(expectedResponse);
        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]

    public async Task Handle_NullRequestValuePassed_SuccessfulResponse_ReturnsData(
        [Frozen] Mock<IApplyApiClient<ApplyApiConfiguration>> apiClientMock,
        GetProvidersAllowedListQueryHandler sut,
        List<AllowedProvider> apiResponse)
    {
        GetProvidersAllowedListQuery query = new();
        var expectedResponse = new GetProvidersAllowedListQueryResponse { Providers = apiResponse };
        apiClientMock.Setup(a => a.GetWithResponseCode<List<AllowedProvider>>(It.Is<GetAllowedProvidersListRequest>(c => c.GetUrl.Equals(new GetAllowedProvidersListRequest(query.sortColumn, query.sortOrder).GetUrl)))).ReturnsAsync(new ApiResponse<List<AllowedProvider>>(apiResponse, HttpStatusCode.OK, ""));

        var result = await sut.Handle(query, CancellationToken.None);

        apiClientMock.Verify(a => a.GetWithResponseCode<List<AllowedProvider>>(It.Is<GetAllowedProvidersListRequest>(c => c.GetUrl.Equals(new GetAllowedProvidersListRequest(query.sortColumn, query.sortOrder).GetUrl))), Times.Once());
        result.Should().BeEquivalentTo(expectedResponse);
        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IApplyApiClient<ApplyApiConfiguration>> apiClientMock,
        GetProvidersAllowedListQuery query,
        GetProvidersAllowedListQueryHandler sut)
    {
        var apiResponse = new ApiResponse<List<AllowedProvider>>(It.IsAny<List<AllowedProvider>>(), HttpStatusCode.InternalServerError, "");
        apiClientMock.Setup(a => a.GetWithResponseCode<List<AllowedProvider>>(It.IsAny<GetAllowedProvidersListRequest>())).ReturnsAsync(apiResponse);

        Func<Task> result = () => sut.Handle(query, It.IsAny<CancellationToken>());

        await result.Should().ThrowAsync<ApiResponseException>();
    }
}