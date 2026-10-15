using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.UpdateOrganisationCourseTypes;

public class UpdateOrganisationCourseTypesCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_CallsV1ApiClient_ToAddCourseTypes(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpV1ApiClient,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCMApiClient,
        UpdateOrganisationCourseTypesCommandHandler sut,
        UpdateOrganisationCourseTypesCommand command,
        NullResponse nullResponse)
    {
        var updateCourseTypes = new UpdateCourseTypesModel(command.CourseTypeIds, command.UserId);
        var v1ApiRequest = new UpdateCourseTypesRequest(command.Ukprn, updateCourseTypes);
        roatpV1ApiClient.Setup(a => a.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r => r.PutUrl.Equals(v1ApiRequest.PutUrl)))).ReturnsAsync(new ApiResponse<NullResponse>(nullResponse, HttpStatusCode.OK, ""));

        roatpCMApiClient.Setup(s => s.PostWithResponseCode<NullResponse>(It.IsAny<AddCourseTypesRequest>())).ReturnsAsync(new ApiResponse<NullResponse>(nullResponse, HttpStatusCode.OK, ""));

        await sut.Handle(command, CancellationToken.None);

        roatpV1ApiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r => r.PutUrl.Equals(v1ApiRequest.PutUrl))), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_V1ApiClientReturnsSuccessfulResponse_PostsCourseTypesToCMApi(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpV1ApiClient,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCMApiClient,
        UpdateOrganisationCourseTypesCommandHandler sut,
        UpdateOrganisationCourseTypesCommand command,
        NullResponse nullResponse)
    {
        var updateCourseTypes = new UpdateCourseTypesModel(command.CourseTypeIds, command.UserId);
        var v1ApiRequest = new UpdateCourseTypesRequest(command.Ukprn, updateCourseTypes);
        roatpV1ApiClient.Setup(a => a.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r => r.PutUrl.Equals(v1ApiRequest.PutUrl)))).ReturnsAsync(new ApiResponse<NullResponse>(nullResponse, HttpStatusCode.OK, ""));

        roatpCMApiClient.Setup(s => s.PostWithResponseCode<NullResponse>(It.IsAny<AddCourseTypesRequest>())).ReturnsAsync(new ApiResponse<NullResponse>(nullResponse, HttpStatusCode.OK, ""));

        await sut.Handle(command, CancellationToken.None);

        roatpV1ApiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r => r.PutUrl.Equals(v1ApiRequest.PutUrl))), Times.Once);
        roatpCMApiClient.Verify(s => s.PostWithResponseCode<NullResponse>(It.IsAny<AddCourseTypesRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_V1ApiClientReturnsSuccessfulResponse_AndCMApiReturnsUnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> roatpV1ApiClient,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCMApiClient,
        UpdateOrganisationCourseTypesCommandHandler sut,
        UpdateOrganisationCourseTypesCommand command,
        NullResponse nullResponse)
    {
        var updateCourseTypes = new UpdateCourseTypesModel(command.CourseTypeIds, command.UserId);
        var v1ApiRequest = new UpdateCourseTypesRequest(command.Ukprn, updateCourseTypes);
        roatpV1ApiClient.Setup(a => a.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r => r.PutUrl.Equals(v1ApiRequest.PutUrl)))).ReturnsAsync(new ApiResponse<NullResponse>(nullResponse, HttpStatusCode.OK, ""));

        roatpCMApiClient.Setup(s => s.PostWithResponseCode<NullResponse>(It.IsAny<AddCourseTypesRequest>())).ReturnsAsync(new ApiResponse<NullResponse>(nullResponse, HttpStatusCode.BadRequest, ""));

        Func<Task> result = () => sut.Handle(command, It.IsAny<CancellationToken>());

        await result.Should().ThrowAsync<ApiResponseException>();

        roatpV1ApiClient.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r => r.PutUrl.Equals(v1ApiRequest.PutUrl))), Times.Once);
        roatpCMApiClient.Verify(s => s.PostWithResponseCode<NullResponse>(It.IsAny<AddCourseTypesRequest>()), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_V1ApiReturnsUnsuccessfulResponse_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> roatpCMApiClient,
        UpdateOrganisationCourseTypesCommandHandler sut,
        UpdateOrganisationCourseTypesCommand command)
    {
        var apiResponse = new ApiResponse<NullResponse>(It.IsAny<NullResponse>(), HttpStatusCode.InternalServerError, "");
        apiClientMock.Setup(a => a.PutWithResponseCode<NullResponse>(It.IsAny<UpdateCourseTypesRequest>())).ReturnsAsync(apiResponse);

        Func<Task> result = () => sut.Handle(command, It.IsAny<CancellationToken>());

        await result.Should().ThrowAsync<ApiResponseException>();
        roatpCMApiClient.Verify(s => s.PostWithResponseCode<NullResponse>(It.IsAny<AddCourseTypesRequest>()), Times.Never);
    }
}
