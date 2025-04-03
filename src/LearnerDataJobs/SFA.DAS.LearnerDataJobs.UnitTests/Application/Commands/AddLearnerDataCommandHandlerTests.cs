using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerDataJobs.UnitTests.Application.Commands;

public class AddLearnerDataCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_AddingANewLearner_Returns_200_Range_HttpStatusCode(
        AddLearnerDataCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] AddLearnerDataCommandHandler handler)
    {
        client.Setup(x =>
                x.PostWithResponseCode<object>(
                    It.Is<PostLearnerDataRequest>(p => p.Data == command.LearnerData && p.PostUrl == "api/learners"),
                    false))
            .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.Created, ""));

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
                x.PostWithResponseCode<object>(It.IsAny<PostLearnerDataRequest>(),
                    false))
            .ReturnsAsync(new ApiResponse<object>(null, HttpStatusCode.BadRequest, "Error"));

        var result = await handler.Handle(command, CancellationToken.None);

        result.Should().BeFalse();
    }

    [Test, MoqAutoData]
    public async Task Then_AddingANewLearner_Throws_Exception(
        AddLearnerDataCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] AddLearnerDataCommandHandler handler)
    {
        client.Setup(x =>
                x.PostWithResponseCode<object>(It.IsAny<PostLearnerDataRequest>(),
                    false)).Throws(new ApplicationException("test"));

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationException>().WithMessage<ApplicationException>("test");
    }
}