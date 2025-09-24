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
public class GetOrganisationsQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_SuccessfulResponse_ReturnsData(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationsQuery query,
        SearchOrganisationResponse apiResponse,
        GetOrganisationsQueryHandler sut
        )
    {
        var expectedResponse = new GetOrganisationsQueryResponse { Organisations = apiResponse.SearchResults.Select(c => (AdminRoatp.Application.Queries.GetOrganisation.Organisation)c) };
        apiClientMock.Setup(a => a.GetWithResponseCode<SearchOrganisationResponse>(It.Is<SearchOrganisationRequest>(c => c.GetUrl.Equals(new SearchOrganisationRequest(query.SearchTerm).GetUrl)))).ReturnsAsync(new ApiResponse<SearchOrganisationResponse>(apiResponse, HttpStatusCode.OK, ""));

        var result = await sut.Handle(query, CancellationToken.None);

        apiClientMock.Verify(a => a.GetWithResponseCode<SearchOrganisationResponse>(It.Is<SearchOrganisationRequest>(c => c.GetUrl.Equals(new SearchOrganisationRequest(query.SearchTerm).GetUrl))), Times.Once);
        result.Organisations.Should().BeEquivalentTo(expectedResponse.Organisations);
        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationsQuery query,
        GetOrganisationsQueryHandler sut)
    {
        var apiResponse = new ApiResponse<SearchOrganisationResponse>(It.IsAny<SearchOrganisationResponse>(), HttpStatusCode.InternalServerError, "");
        apiClientMock.Setup(a => a.GetWithResponseCode<SearchOrganisationResponse>(It.IsAny<SearchOrganisationRequest>())).ReturnsAsync(apiResponse);

        Func<Task> result = () => sut.Handle(query, It.IsAny<CancellationToken>());

        await result.Should().ThrowAsync<ApiResponseException>();
    }
}
