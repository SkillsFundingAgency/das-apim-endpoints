using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.Application.Handlers;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerDataJobs.UnitTests.Application.Commands;

public class AddLearnerDataCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_AddingANewLearner_Returns_200_Range_HttpStatusCode(
        AddLearnerDataCommand command,
        StandardDetailResponse courseResponse,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Frozen] Mock<IInternalApiClient<CoursesApiConfiguration>> courseClient,
        [Greedy] AddLearnerDataCommandHandler handler)
    {

        courseResponse.ApprenticeshipType = "Apprenticeship";

        var expectedUrl =
            $"providers/{command.LearnerData.UKPRN}/learners/{command.LearnerData.ULN}";
        client.Setup(x =>
                x.PutWithResponseCode<NullResponse>(
                    It.Is<PutLearnerDataRequest>(p => p.PutUrl == expectedUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.Created, ""));

        courseClient.Setup(x =>
                x.Get<StandardDetailResponse>(
                    It.Is<GetStandardDetailsByIdRequest>(p => p.Id == command.LearnerData.LarsCode)))
            .ReturnsAsync(courseResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeTrue();
    }

    [Test, MoqAutoData]
    public async Task Then_AddingANewLearner_Returns_Non200_Range_HttpStatusCode(
        AddLearnerDataCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] AddLearnerDataCommandHandler handler)
    {
        client.Setup(x =>
                x.PutWithResponseCode<NullResponse>(It.IsAny<PutLearnerDataRequest>()))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.BadRequest, "Error"));

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_AddingANewLearner_Throws_Exception(
        StandardDetailResponse courseResponse,
        AddLearnerDataCommand command,
        [Frozen] Mock<IInternalApiClient<CoursesApiConfiguration>> courseClient,
        [Greedy] AddLearnerDataCommandHandler handler)
    {
        courseResponse.ApprenticeshipType = "Unknown";


        courseClient.Setup(x =>
                x.Get<StandardDetailResponse?>(
                    It.Is<GetStandardDetailsByIdRequest>(p => p.Id == command.LearnerData.LarsCode)))
            .ReturnsAsync(courseResponse);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentException>().WithMessage("'Unknown' is not a valid description for LearningType");
    }

    [Test, MoqAutoData]
    public async Task Then_AddingANewLearner_Throws_Exception_When_LarsCode_Is_Blank(
        AddLearnerDataCommand command,
        [Greedy] AddLearnerDataCommandHandler handler)
    {
        command.LearnerData.LarsCode = string.Empty;

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ArgumentNullException>().WithMessage("Learner data LarsCode cannot be null (Parameter 'LearnerData')");
    }

    [Test, MoqAutoData]
    public async Task Then_AddingANewLearner_Throws_Exception_When_No_CourseFound(
    AddLearnerDataCommand command,
    [Frozen] Mock<IInternalApiClient<CoursesApiConfiguration>> courseClient,
    [Greedy] AddLearnerDataCommandHandler handler)
    {
        StandardDetailResponse? courseResponse = null;

        courseClient.Setup(x =>
                x.Get<StandardDetailResponse?>(
                    It.Is<GetStandardDetailsByIdRequest>(p => p.Id == command.LearnerData.LarsCode)))
            .ReturnsAsync(courseResponse);

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<Exception>().WithMessage($"No course found for LARS code {command.LearnerData.LarsCode}");
    }

    [Test, MoqAutoData]
    public async Task Then_AddingANewLearner_Maps_To_Inner_Api_Successfully(
        AddLearnerDataCommand command,
        StandardDetailResponse courseResponse,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Frozen] Mock<IInternalApiClient<CoursesApiConfiguration>> courseClient,
        [Greedy] AddLearnerDataCommandHandler handler)
    {

        courseResponse.ApprenticeshipType = "Apprenticeship";

        var expectedUrl =
            $"providers/{command.LearnerData.UKPRN}/learners/{command.LearnerData.ULN}";
        client.Setup(x =>
                x.PutWithResponseCode<NullResponse>(
                    It.Is<PutLearnerDataRequest>(p => p.PutUrl == expectedUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.Created, ""));

        courseClient.Setup(x =>
                x.Get<StandardDetailResponse>(
                    It.Is<GetStandardDetailsByIdRequest>(p => p.Id == command.LearnerData.LarsCode)))
            .ReturnsAsync(courseResponse);

        var result = await handler.Handle(command, CancellationToken.None);

        client.Verify(x => x.PutWithResponseCode<NullResponse>(It.Is<PutLearnerDataRequest>(p => ((LearnerDataRequest)p.Data).LarsCode == command.LearnerData.LarsCode && ((LearnerDataRequest)p.Data).TrainingName == courseResponse.Title)));
    }
}