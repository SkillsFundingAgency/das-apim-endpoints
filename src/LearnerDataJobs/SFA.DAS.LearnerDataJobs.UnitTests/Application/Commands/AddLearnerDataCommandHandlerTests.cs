using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Infrastructure;
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

        var expectedUrl =
            $"providers/{command.LearnerData.UKPRN}/learners/{command.LearnerData.ULN}";
        client.Setup(x =>
                x.PutWithResponseCode<NullResponse>(
                    It.Is<PutLearnerDataRequest>(p => p.Data == command.LearnerData && p.PutUrl == expectedUrl)))
            .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.Created, ""));

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
        AddLearnerDataCommand command,
        [Frozen] Mock<IInternalApiClient<LearnerDataInnerApiConfiguration>> client,
        [Greedy] AddLearnerDataCommandHandler handler)
    {
        client.Setup(x =>
                x.PutWithResponseCode<object>(It.IsAny<PutLearnerDataRequest>()))
            .Throws(new ApplicationException("test"));

        var act = async () => await handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<ApplicationException>().WithMessage<ApplicationException>("test");
    }
}