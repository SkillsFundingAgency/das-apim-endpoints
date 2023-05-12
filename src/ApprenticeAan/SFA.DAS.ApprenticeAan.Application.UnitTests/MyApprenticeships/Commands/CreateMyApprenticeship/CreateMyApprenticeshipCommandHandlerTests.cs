using System.Linq.Expressions;
using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeships.Commands.CreateMyApprenticeships;

public class CreateMyApprenticeshipCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesAccountsApi(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
        CreateMyApprenticeshipCommandHandler sut,
        CreateMyApprenticeshipCommand command,
        CancellationToken cancellationToken)
    {
        ApiResponse<object> result = new(this, HttpStatusCode.Created, null);
        apiClientMock.Setup(c => c.PostWithResponseCode<object>(It.IsAny<PostMyApprenticeshipRequest>(), false)).ReturnsAsync(result);

        await sut.Handle(command, cancellationToken);

        Expression<Func<PostMyApprenticeshipRequest, bool>> IsCorrectRequest = r => r.PostUrl == $"apprentices/{command.ApprenticeId}/MyApprenticeship" && r.Data == command;

        apiClientMock
            .Verify(c => c.PostWithResponseCode<object>(It.Is(IsCorrectRequest), false));
    }

    [Test, MoqAutoData]
    public async Task Handle_OnSuccess_ReturnsUnit(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
        CreateMyApprenticeshipCommandHandler sut,
        CreateMyApprenticeshipCommand command)
    {
        ApiResponse<object> response = new(this, HttpStatusCode.Created, null);
        apiClientMock.Setup(c => c.PostWithResponseCode<object>(It.IsAny<PostMyApprenticeshipRequest>(), false)).ReturnsAsync(response);

        var result = await sut.Handle(command, new CancellationToken());

        result.Should().NotBeNull();
    }

    [Test, MoqAutoData]
    public async Task Handle_OnFailure_ThrowsException(
        [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> apiClientMock,
        CreateMyApprenticeshipCommandHandler sut,
        CreateMyApprenticeshipCommand command)
    {
        ApiResponse<object> response = new(this, HttpStatusCode.BadRequest, null);
        apiClientMock.Setup(c => c.PostWithResponseCode<object>(It.IsAny<PostMyApprenticeshipRequest>(), false)).ReturnsAsync(response);

        Func<Task> action = () => sut.Handle(command, new CancellationToken());

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
