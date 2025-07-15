using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
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
using SFA.DAS.Testing.AutoFixture;
using AutoFixture.NUnit3;

namespace SFA.DAS.Approvals.UnitTests.Application.Apprentices.Commands;

[TestFixture]
public class ConfirmEditApprenticeshipCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_WhenApprenticeshipFound_ShouldFetchCourseRulesAndEditApprenticeship(
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
        commitmentsV2ApiClient.Setup(x => x.Post<EditApprenticeshipResponse>(It.IsAny<EditApprenticeshipApiRequest>()))
            .ReturnsAsync(editResponse);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.ApprenticeshipId.Should().Be(editResponse.ApprenticeshipId);
        result.NeedReapproval.Should().Be(editResponse.NeedReapproval);
        courseTypeRulesService.Verify(x => x.GetCourseTypeRulesAsync(command.CourseCode), Times.Once);
        commitmentsV2ApiClient.Verify(x => x.Post<EditApprenticeshipResponse>(It.IsAny<EditApprenticeshipApiRequest>()), Times.Once);
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
        commitmentsV2ApiClient.Setup(x => x.Post<EditApprenticeshipResponse>(It.Is<EditApprenticeshipApiRequest>(r => ((EditApprenticeshipApiRequestData)r.Data).Option == string.Empty)))
            .ReturnsAsync(editResponse);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        commitmentsV2ApiClient.Verify(x => x.Post<EditApprenticeshipResponse>(It.Is<EditApprenticeshipApiRequest>(r => ((EditApprenticeshipApiRequestData)r.Data).Option == string.Empty)), Times.Once);
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
        commitmentsV2ApiClient.Setup(x => x.Post<EditApprenticeshipResponse>(It.IsAny<EditApprenticeshipApiRequest>()))
            .ThrowsAsync(new HttpRequestContentException("fail", System.Net.HttpStatusCode.BadRequest, "fail"));

        // Act & Assert
        await FluentActions.Invoking(() => handler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<HttpRequestContentException>();
    }
} 