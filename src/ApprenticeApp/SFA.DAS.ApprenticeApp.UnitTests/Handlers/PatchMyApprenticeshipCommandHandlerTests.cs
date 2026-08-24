using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.ApprenticeApp.Application.Commands.ApprenticeAccounts;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class PatchMyApprenticeshipCommandHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Creates_Patch_Request_And_Returns_True(
    Guid apprenticeId,
    object patchData,
    [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> client)
        {
            var command = new PatchMyApprenticeshipCommand
            {
                ApprenticeId = apprenticeId,
                PatchData = patchData
            };

            IPatchApiRequest<object> actualRequest = null;

            client
                .Setup(x => x.Patch(It.IsAny<IPatchApiRequest<object>>()))
                .Callback<IPatchApiRequest<object>>(request =>
                    actualRequest = request)
                .Returns(Task.CompletedTask);

            var handler = new PatchMyApprenticeshipCommandHandler(client.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().BeTrue();

            actualRequest.Should().NotBeNull();
            actualRequest.Should().BeOfType<PatchMyApprenticeshipRequest>();
            actualRequest.PatchUrl.Should()
                .Be($"apprentice/{apprenticeId}/MyApprenticeship");
            actualRequest.Data.Should().BeSameAs(patchData);

            client.Verify(x => x.Patch(
                It.Is<IPatchApiRequest<object>>(request =>
                    request.PatchUrl ==
                        $"apprentice/{apprenticeId}/MyApprenticeship" &&
                    request.Data == patchData)),
                Times.Once);
        }
    }
}
