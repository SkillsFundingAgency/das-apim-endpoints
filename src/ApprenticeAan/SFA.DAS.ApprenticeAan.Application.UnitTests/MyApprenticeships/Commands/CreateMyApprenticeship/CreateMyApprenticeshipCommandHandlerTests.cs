using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeships.Commands.CreateMyApprenticeship;

public class CreateMyApprenticeshipCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesAccountsApi(
        [Frozen] Mock<IApprenticeAccountsApiClient> apiClientMock,
        CreateMyApprenticeshipCommandHandler sut,
        CreateMyApprenticeshipCommand command,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(c => c.PostMyApprenticeship(command.ApprenticeId, command, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created));

        await sut.Handle(command, cancellationToken);

        apiClientMock
             .Verify(c => c.PostMyApprenticeship(command.ApprenticeId, command, It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Handle_OnSuccess_ReturnsUnit(
        [Frozen] Mock<IApprenticeAccountsApiClient> apiClientMock,
        CreateMyApprenticeshipCommandHandler sut,
        CreateMyApprenticeshipCommand command)
    {
        apiClientMock.Setup(c => c.PostMyApprenticeship(command.ApprenticeId, command, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Created));
        var result = await sut.Handle(command, new CancellationToken());
        result.Should().NotBeNull();
    }


    [Test, MoqAutoData]
    public void Handle_OnFailure_ThrowsInvalidOperationException(
        [Frozen] Mock<IApprenticeAccountsApiClient> apiClientMock,
        CreateMyApprenticeshipCommandHandler sut,
        CreateMyApprenticeshipCommand command)
    {
        apiClientMock.Setup(c => c.PostMyApprenticeship(command.ApprenticeId, command, It.IsAny<CancellationToken>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.Ambiguous));
        Assert.That(() => sut.Handle(command, It.IsAny<CancellationToken>()), Throws.InvalidOperationException);
    }
}