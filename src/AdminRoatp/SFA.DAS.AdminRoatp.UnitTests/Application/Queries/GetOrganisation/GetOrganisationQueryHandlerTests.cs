using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Exceptions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisation;
public class GetOrganisationQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_SuccessfulResponse_ReturnsData(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClient,
        GetOrganisationQueryHandler sut,
        GetOrganisationQuery request,
        OrganisationResponse apiResponse)
    {
        apiResponse.Status = OrganisationStatus.Active;
        apiClient.Setup(a => a.GetWithResponseCode<OrganisationResponse>(It.Is<GetOrganisationRequest>(r => r.GetUrl.Equals(new GetOrganisationRequest(request.ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<OrganisationResponse>(apiResponse, HttpStatusCode.OK, ""));

        GetOrganisationQueryResult? result = await sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
    }

    [Test, MoqAutoData]
    public async Task Handle_Unsuccessful404Response_ReturnNullResponse(
    [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
    GetOrganisationQuery query,
    GetOrganisationQueryHandler sut)
    {
        var apiResponse = new ApiResponse<OrganisationResponse>(It.IsAny<OrganisationResponse>(), HttpStatusCode.NotFound, "");
        apiClientMock.Setup(a => a.GetWithResponseCode<OrganisationResponse>(It.IsAny<GetOrganisationRequest>())).ReturnsAsync(apiResponse);

        var result = await sut.Handle(query, It.IsAny<CancellationToken>());

        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_UnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationQuery query,
        GetOrganisationQueryHandler sut)
    {
        var apiResponse = new ApiResponse<OrganisationResponse>(It.IsAny<OrganisationResponse>(), HttpStatusCode.InternalServerError, "");
        apiClientMock.Setup(a => a.GetWithResponseCode<OrganisationResponse>(It.IsAny<GetOrganisationRequest>())).ReturnsAsync(apiResponse);

        Func<Task> result = () => sut.Handle(query, It.IsAny<CancellationToken>());

        await result.Should().ThrowAsync<ApiResponseException>();
    }

    [Test, MoqAutoData]
    public async Task Handle_OrganisationHasRemovedStatus_ReturnsDataWithRemovedDate(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationQueryHandler sut,
        GetOrganisationQuery request,
        OrganisationResponse apiResponse,
        DateTime removedDate)
    {
        apiResponse.Status = OrganisationStatus.Removed;
        apiClientMock.Setup(a => a.GetWithResponseCode<OrganisationResponse>(It.Is<GetOrganisationRequest>(r => r.GetUrl.Equals(new GetOrganisationRequest(request.ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<OrganisationResponse>(apiResponse, HttpStatusCode.OK, ""));

        GetOrganisationStatusHistoryResponse historyResponse = new([
            new StatusHistoryModel(OrganisationStatus.Removed, removedDate.AddDays(-1)),
            new StatusHistoryModel(OrganisationStatus.Removed, removedDate)
        ]);
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationStatusHistoryResponse>(It.Is<GetOrganisationStatusHistoryRequest>(r => r.Ukprn == request.ukprn))).ReturnsAsync(new ApiResponse<GetOrganisationStatusHistoryResponse>(historyResponse, HttpStatusCode.OK, ""));

        GetOrganisationQueryResult? result = await sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        result!.RemovedDate.Should().Be(removedDate);
    }

    [Test, MoqAutoData]
    public async Task Handle_OrganisationHasRemovedStatus_WithNoMatchingHistory_ReturnsDataWithNoRemovedDate(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationQueryHandler sut,
        GetOrganisationQuery request,
        OrganisationResponse apiResponse)
    {
        apiResponse.Status = OrganisationStatus.Removed;
        apiClientMock.Setup(a => a.GetWithResponseCode<OrganisationResponse>(It.Is<GetOrganisationRequest>(r => r.GetUrl.Equals(new GetOrganisationRequest(request.ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<OrganisationResponse>(apiResponse, HttpStatusCode.OK, ""));

        GetOrganisationStatusHistoryResponse historyResponse = new([
            new StatusHistoryModel(OrganisationStatus.Active, DateTime.UtcNow),
            new StatusHistoryModel(OrganisationStatus.OnBoarding, DateTime.Today)
        ]);
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationStatusHistoryResponse>(It.Is<GetOrganisationStatusHistoryRequest>(r => r.Ukprn == request.ukprn))).ReturnsAsync(new ApiResponse<GetOrganisationStatusHistoryResponse>(historyResponse, HttpStatusCode.OK, ""));

        GetOrganisationQueryResult? result = await sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        result!.RemovedDate.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_OrganisationHasRemovedStatus_WithNoStatusHistory_ReturnsDataWithNoRemovedDate(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationQueryHandler sut,
        GetOrganisationQuery request,
        OrganisationResponse apiResponse)
    {
        apiResponse.Status = OrganisationStatus.Removed;
        apiClientMock.Setup(a => a.GetWithResponseCode<OrganisationResponse>(It.Is<GetOrganisationRequest>(r => r.GetUrl.Equals(new GetOrganisationRequest(request.ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<OrganisationResponse>(apiResponse, HttpStatusCode.OK, ""));

        GetOrganisationStatusHistoryResponse historyResponse = new([]);
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationStatusHistoryResponse>(It.Is<GetOrganisationStatusHistoryRequest>(r => r.Ukprn == request.ukprn))).ReturnsAsync(new ApiResponse<GetOrganisationStatusHistoryResponse>(historyResponse, HttpStatusCode.OK, ""));

        GetOrganisationQueryResult? result = await sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        result!.RemovedDate.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_OrganisationHasRemovedStatus_StatusHistoryHasFailureResponse_ReturnsDataWithNoRemovedDate(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        GetOrganisationQueryHandler sut,
        GetOrganisationQuery request,
        OrganisationResponse apiResponse)
    {
        apiResponse.Status = OrganisationStatus.Removed;
        apiClientMock.Setup(a => a.GetWithResponseCode<OrganisationResponse>(It.Is<GetOrganisationRequest>(r => r.GetUrl.Equals(new GetOrganisationRequest(request.ukprn).GetUrl)))).ReturnsAsync(new ApiResponse<OrganisationResponse>(apiResponse, HttpStatusCode.OK, ""));

        GetOrganisationStatusHistoryResponse historyResponse = new([]);
        apiClientMock.Setup(a => a.GetWithResponseCode<GetOrganisationStatusHistoryResponse>(It.Is<GetOrganisationStatusHistoryRequest>(r => r.Ukprn == request.ukprn))).ReturnsAsync(new ApiResponse<GetOrganisationStatusHistoryResponse>(historyResponse, HttpStatusCode.InternalServerError, ""));

        GetOrganisationQueryResult? result = await sut.Handle(request, CancellationToken.None);

        result.Should().BeEquivalentTo(apiResponse, options => options.ExcludingMissingMembers());
        result!.RemovedDate.Should().BeNull();
    }
}

