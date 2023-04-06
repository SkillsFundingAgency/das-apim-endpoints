using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;
using SFA.DAS.RoatpOversight.Application.Providers.Commands.CreateProvider;
using SFA.DAS.RoatpOversight.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.RoatpOversight.UnitTests.Commands.CreateProvider;
public class CreateProviderCommandHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handler_InvokesApi(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        ApiResponse<int> apiResponse = new(1, HttpStatusCode.Created, null);

        apiClientMock.Setup(c => c.PostWithResponseCode<int>(It.IsAny<CreateProviderRequest>(), It.IsAny<bool>())).ReturnsAsync(apiResponse);

        await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.PostWithResponseCode<int>(It.Is<CreateProviderRequest>(r => r.Data == command), true));
    }

    [Test]
    [MoqAutoData]
    public async Task Handler_UnexpectedApiResponse_ThrowsInvalidOperation(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
        CreateProviderCommandHandler sut,
        CreateProviderCommand command,
        CancellationToken cancellationToken)
    {
        ApiResponse<int> apiResponse = new(1, HttpStatusCode.BadRequest, null);

        apiClientMock.Setup(c => c.PostWithResponseCode<int>(It.IsAny<CreateProviderRequest>(), It.IsAny<bool>())).ReturnsAsync(apiResponse);

        Func<Task> action = () => sut.Handle(command, cancellationToken);

        await action.Should().ThrowAsync<InvalidOperationException>();
    }
}
