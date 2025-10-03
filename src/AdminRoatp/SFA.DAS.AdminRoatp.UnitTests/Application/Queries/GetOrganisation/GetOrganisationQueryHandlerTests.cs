using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisation;
public class GetOrganisationQueryHandlerTests
{
    [Test, MoqAutoData]

    public async Task Handle_SuccessfulResponse_ReturnsData(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetOrganisationQueryHandler sut,
        GetOrganisationQuery request,
        GetOrganisationResponse apiResponse)
    {
        apiClient.Setup(a => a.GetWithResponseCode<GetOrganisationResponse>(It.Is<GetOrganisationRequest>(r => r.GetUrl.Equals(new GetOrganisationRequest(request.ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<GetOrganisationResponse>(apiResponse, HttpStatusCode.OK, ""));

        var result = await sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Handle_Unsuccessful404Response_ReturnNullResponse(
    [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
    GetOrganisationQuery query,
    GetOrganisationQueryHandler sut)
    {
        var apiResponse = new ApiResponse<GetOrganisationResponse>(It.IsAny<GetOrganisationResponse>(), HttpStatusCode.NotFound, "");
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationResponse>(It.IsAny<GetOrganisationRequest>())).ReturnsAsync(apiResponse);

        var result = await sut.Handle(query, It.IsAny<CancellationToken>());

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationQuery query,
        GetOrganisationQueryHandler sut)
    {
        var apiResponse = new ApiResponse<GetOrganisationResponse>(It.IsAny<GetOrganisationResponse>(), HttpStatusCode.InternalServerError, "");
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationResponse>(It.IsAny<GetOrganisationRequest>())).ReturnsAsync(apiResponse);

        Func<Task> result = () => sut.Handle(query, It.IsAny<CancellationToken>());

        await result.Should().ThrowAsync<ApiResponseException>();
    }
}