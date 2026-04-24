using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Exceptions;
using SFA.DAS.Apim.Shared.Infrastructure;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.Apim.Shared.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.UpdateOrganisationCourseTypes;
public class UpdateOrganisationCourseTypesCommandHandlerTests
{
    [Test, MoqAutoData]

    public async Task Handle_CallsApiClient_EnsuresSuccessResponse(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        UpdateOrganisationCourseTypesCommandHandler sut,
        UpdateOrganisationCourseTypesCommand command,
        NullResponse apiResponse)
    {
        var updateCourseTypes = new UpdateCourseTypesModel(command.CourseTypeIds, command.UserId);
        var apiRequest = new UpdateCourseTypesRequest(command.ukprn, updateCourseTypes);
        apiClientMock.Setup(a => a.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r => r.PutUrl.Equals(apiRequest.PutUrl)))).ReturnsAsync(new ApiResponse<NullResponse>(apiResponse, HttpStatusCode.OK, ""));

        Func<Task> result = () => sut.Handle(command, CancellationToken.None);

        await result.Should().NotThrowAsync();
        apiClientMock.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<UpdateCourseTypesRequest>(r => r.PutUrl.Equals(apiRequest.PutUrl))), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_CallsApiClient_ThrowsException(
        [Frozen] Mock<IRoatpServiceApiClient<RoatpConfiguration>> apiClientMock,
        UpdateOrganisationCourseTypesCommandHandler sut,
        UpdateOrganisationCourseTypesCommand command)
    {
        var apiResponse = new ApiResponse<NullResponse>(It.IsAny<NullResponse>(), HttpStatusCode.InternalServerError, "");
        apiClientMock.Setup(a => a.PutWithResponseCode<NullResponse>(It.IsAny<UpdateCourseTypesRequest>())).ReturnsAsync(apiResponse);

        Func<Task> result = () => sut.Handle(command, It.IsAny<CancellationToken>());

        await result.Should().ThrowAsync<ApiResponseException>();
    }
}