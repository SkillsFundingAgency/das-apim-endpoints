using System.Net;
using AutoFixture.NUnit4;
using FluentAssertions;
using Moq;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;
using SFA.DAS.RoatpOversight.Infrastructure;
using SFA.DAS.RoatpOversight.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpOversight.UnitTests.Commands.CreateProvider;

public class CreateProviderCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handler_WhenProviderDoesNotExist_InvokesCreateApi(
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(x => x.GetProvider(command.Ukprn)).ReturnsAsync(new HttpResponseMessage
        { StatusCode = HttpStatusCode.BadRequest });

        apiClientMock.Setup(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Created, Version = new Version() });

        apiClientMock.Setup(c => c.AddCourseTypes(command.Ukprn, It.IsAny<AddCourseTypesModel>(), cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent, Version = new Version() });

        await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handler_WhenProviderExists_DoesNotInvokeCreateApi(
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(x => x.GetProvider(command.Ukprn)).ReturnsAsync(new HttpResponseMessage
        { StatusCode = HttpStatusCode.NoContent });

        await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task Handler_UnexpectedApiResponse_ThrowsInvalidOperation(
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(x => x.GetProvider(command.Ukprn)).ReturnsAsync(new HttpResponseMessage
        { StatusCode = HttpStatusCode.BadRequest });

        apiClientMock.Setup(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, Version = new Version() });


        Func<Task> action = () => sut.Handle(command, cancellationToken);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }

    [Test, MoqAutoData]
    public async Task Handler_WhenProviderAdded_ThenAddsApprenticeshipCourseType(
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        apiClientMock.Setup(x => x.GetProvider(command.Ukprn)).ReturnsAsync(new HttpResponseMessage
        { StatusCode = HttpStatusCode.BadRequest });

        apiClientMock.Setup(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Created, Version = new Version() });

        apiClientMock.Setup(c => c.AddCourseTypes(command.Ukprn, It.IsAny<AddCourseTypesModel>(), cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NoContent, Version = new Version() });

        await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.AddCourseTypes(command.Ukprn, It.Is<AddCourseTypesModel>(x => x.CourseTypes.Contains(CourseType.Apprenticeship)), cancellationToken), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handler_WhenAddCourseTypesIsNotSuccessful_ThenThrowsException(
        [Frozen] Mock<IRoatpV2ApiClient> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        var addCourseTypesCommand = new AddCourseTypesModel()
        {
            CourseTypes = [CourseType.Apprenticeship],
            UserId = command.UserId,
            UserDisplayName = command.UserDisplayName
        };

        apiClientMock.Setup(x => x.GetProvider(command.Ukprn)).ReturnsAsync(new HttpResponseMessage
        { StatusCode = HttpStatusCode.BadRequest });

        apiClientMock.Setup(c => c.CreateProvider(command.UserId, command.UserDisplayName, command, cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.Created, Version = new Version() });

        apiClientMock.Setup(c => c.AddCourseTypes(command.Ukprn, addCourseTypesCommand, cancellationToken))
            .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, Version = new Version() });

        Func<Task> action = () => sut.Handle(command, cancellationToken);

        await action.Should().ThrowAsync<HttpRequestException>();
    }
}
