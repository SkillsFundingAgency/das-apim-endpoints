using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Configuration;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;
using SFA.DAS.ApprenticeAan.Application.Services;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Apprentices.Commands.CreateApprenticeMember;

public class CreateApprenticeMemberCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock,
        CreateApprenticeMemberCommandHandler sut,
        CreateApprenticeMemberCommand command)
    {
        ApiResponse<object> result = new(this, HttpStatusCode.Created, null);
        apiClientMock.Setup(c => c.PostWithResponseCode<object>(It.Is<PostApprenticeRequest>(r => r.PostUrl == "apprentices"), true)).ReturnsAsync(result);

        await sut.Handle(command, new CancellationToken());

        apiClientMock.Verify(c => c.PostWithResponseCode<object>(It.Is<PostApprenticeRequest>(r => r.PostUrl == "apprentices"), true));
    }

    [Test, MoqAutoData]
    public async Task Handle_OnSuccess_ReturnsUnit(
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock,
        CreateApprenticeMemberCommandHandler sut,
        CreateApprenticeMemberCommand command)
    {
        ApiResponse<object> response = new(this, HttpStatusCode.Created, null);
        apiClientMock.Setup(c => c.PostWithResponseCode<object>(It.Is<PostApprenticeRequest>(r => r.PostUrl == "apprentices"), true)).ReturnsAsync(response);

        var result = await sut.Handle(command, new CancellationToken());

        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_OnFailure_ThrowsException(
        [Frozen] Mock<IAanHubApiClient<AanHubApiConfiguration>> apiClientMock,
        CreateApprenticeMemberCommandHandler sut,
        CreateApprenticeMemberCommand command)
    {
        ApiResponse<object> response = new(this, HttpStatusCode.BadRequest, null);
        apiClientMock.Setup(c => c.PostWithResponseCode<object>(It.Is<PostApprenticeRequest>(r => r.PostUrl == "apprentices"), true)).ReturnsAsync(response);

        Func<Task> action = () => sut.Handle(command, new CancellationToken());

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
