using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Apprentices.Commands.ConfirmEditApprenticeship;
using SFA.DAS.Approvals.InnerApi.CourseTypesApi.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Collections.Generic;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Commands;

[TestFixture]
public class ConfirmEditApprenticeshipCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_WhenApprenticeshipFound_ShouldFetchCourseRulesForNewCourseCode(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2ApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        ConfirmEditApprenticeshipCommandHandler handler,
        ConfirmEditApprenticeshipCommand command,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        EditApprenticeshipResponse editResponse)
    {
        // Arrange
        commitmentsV2ApiClient.Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);
        courseTypeRulesService.Setup(x => x.GetCourseTypeRulesAsync(command.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult { LearnerAgeRules = learnerAgeResponse });
        
        commitmentsV2ApiClient.Setup(x => x.PostWithResponseCode<EditApprenticeshipResponse>(It.IsAny<EditApprenticeshipApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<EditApprenticeshipResponse>(editResponse, HttpStatusCode.OK, string.Empty, new Dictionary<string, IEnumerable<string>>()));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ApprenticeshipId.Should().Be(editResponse.ApprenticeshipId);
        result.NeedReapproval.Should().Be(editResponse.NeedReapproval);
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(command.CourseCode), Times.Once);
        commitmentsV2ApiClient.Verify(x => x.PostWithResponseCode<EditApprenticeshipResponse>(It.IsAny<EditApprenticeshipApiRequest>(), true), Times.Once);
    }


    [Test, MoqAutoData]
    public async Task Handle_WhenApprenticeshipFound_ShouldFetchCourseRulesForExistingCourseCode(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2ApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        ConfirmEditApprenticeshipCommandHandler handler,
        ConfirmEditApprenticeshipCommand command,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        EditApprenticeshipResponse editResponse)
    {
        // Arrange
        command.CourseCode = null;
        commitmentsV2ApiClient.Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);
        courseTypeRulesService.Setup(x => x.GetCourseTypeRulesAsync(apprenticeshipResponse.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult { LearnerAgeRules = learnerAgeResponse });

        commitmentsV2ApiClient.Setup(x => x.PostWithResponseCode<EditApprenticeshipResponse>(It.IsAny<EditApprenticeshipApiRequest>(), true))
            .ReturnsAsync(new ApiResponse<EditApprenticeshipResponse>(editResponse, HttpStatusCode.OK, string.Empty, new Dictionary<string, IEnumerable<string>>()));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ApprenticeshipId.Should().Be(editResponse.ApprenticeshipId);
        result.NeedReapproval.Should().Be(editResponse.NeedReapproval);
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(apprenticeshipResponse.CourseCode), Times.Once);
        commitmentsV2ApiClient.Verify(x => x.PostWithResponseCode<EditApprenticeshipResponse>(It.IsAny<EditApprenticeshipApiRequest>(), true), Times.Once);
    }



    [Test, MoqAutoData]
    public async Task Handle_WhenOptionIsTBC_ShouldSetEmptyString(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2ApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        ConfirmEditApprenticeshipCommandHandler handler,
        ConfirmEditApprenticeshipCommand command,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse,
        EditApprenticeshipResponse editResponse)
    {
        // Arrange
        command.Option = "TBC";
        commitmentsV2ApiClient.Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);
        courseTypeRulesService.Setup(x => x.GetCourseTypeRulesAsync(command.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult { LearnerAgeRules = learnerAgeResponse });
        
        commitmentsV2ApiClient.Setup(x => x.PostWithResponseCode<EditApprenticeshipResponse>(It.Is<EditApprenticeshipApiRequest>(r => ((EditApprenticeshipApiRequestData)r.Data).Option == string.Empty), true))
            .ReturnsAsync(new ApiResponse<EditApprenticeshipResponse>(editResponse, HttpStatusCode.OK, string.Empty, new Dictionary<string, IEnumerable<string>>()));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        commitmentsV2ApiClient.Verify(x => x.PostWithResponseCode<EditApprenticeshipResponse>(It.Is<EditApprenticeshipApiRequest>(r => ((EditApprenticeshipApiRequestData)r.Data).Option == string.Empty), true), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_WhenEditFails_ShouldThrowException(
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> commitmentsV2ApiClient,
        [Frozen] Mock<ICourseTypeRulesService> courseTypeRulesService,
        ConfirmEditApprenticeshipCommandHandler handler,
        ConfirmEditApprenticeshipCommand command,
        GetApprenticeshipResponse apprenticeshipResponse,
        GetLearnerAgeResponse learnerAgeResponse)
    {
        // Arrange
        commitmentsV2ApiClient.Setup(x => x.Get<GetApprenticeshipResponse>(It.IsAny<GetApprenticeshipRequest>()))
            .ReturnsAsync(apprenticeshipResponse);
        courseTypeRulesService.Setup(x => x.GetCourseTypeRulesAsync(command.CourseCode))
            .ReturnsAsync(new CourseTypeRulesResult { LearnerAgeRules = learnerAgeResponse });
        commitmentsV2ApiClient.Setup(x => x.PostWithResponseCode<EditApprenticeshipResponse>(It.IsAny<EditApprenticeshipApiRequest>(), true))
            .ThrowsAsync(new HttpRequestContentException("fail", HttpStatusCode.BadRequest, "fail"));

        // Act & Assert
        await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<HttpRequestContentException>();
    }
} 