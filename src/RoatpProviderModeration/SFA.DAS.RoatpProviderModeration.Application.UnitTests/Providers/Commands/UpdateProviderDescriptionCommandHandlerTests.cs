using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpProviderModeration.Application.InnerApi.Requests;
using SFA.DAS.RoatpProviderModeration.Application.Provider.Commands.UpdateProviderDescription;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpProviderModeration.Application.UnitTests.Providers.Commands
{
    [TestFixture]
    public class UpdateProviderDescriptionCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateProviderDescriptionCommandHandler sut,
            UpdateProviderDescriptionCommand command,
            CancellationToken cancellationToken)
        {
            await sut.Handle(command, cancellationToken);
            apiClientMock.Verify(a => a.PatchWithResponseCode(It.IsAny<PatchProviderRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public void Handle_CallsApiClient_ReturnException(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateProviderDescriptionCommandHandler sut,
            UpdateProviderDescriptionCommand command,
            CancellationToken cancellationToken,
            HttpRequestContentException expectedException)
        {
            apiClientMock.Setup(c => c.PatchWithResponseCode(It.IsAny<PatchProviderRequest>())).Throws(expectedException);

            var actualException = Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(command, cancellationToken));

            actualException.Should().Be(expectedException);
        }
    }
}
