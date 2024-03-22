using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.UpdateProviderLocationDetails;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.RoatpCourseManagement.UnitTests.Application.Locations.Commands.UpdateProviderLocationDetails
{
    [TestFixture]
    public class UpdateProviderLocationDetailsCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_CallsApiClient(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateProviderLocationDetailsCommandHandler sut,
            UpdateProviderLocationDetailsCommand command,
            CancellationToken cancellationToken)
        {
            await sut.Handle(command, cancellationToken);

            apiClientMock.Verify(a => a.Put(It.IsAny<ProviderLocationUpdateRequest>()), Times.Once);
        }

        [Test, MoqAutoData]
        public void Handle_CallsApiClient_ReturnException(
            [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClientMock,
            UpdateProviderLocationDetailsCommandHandler sut,
            UpdateProviderLocationDetailsCommand command,
            CancellationToken cancellationToken,
            HttpRequestContentException expectedException)
        {
            apiClientMock.Setup(c => c.Put(It.IsAny<ProviderLocationUpdateRequest>())).Throws(expectedException);

            var actualException = Assert.ThrowsAsync<HttpRequestContentException>(() => sut.Handle(command, cancellationToken));

            actualException.Should().Be(expectedException);
        }
    }
}
