using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Queries.GetApprenticeTrainingProviders;
using SFA.DAS.Testing.AutoFixture;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenGettingApprenticeTrainingProviders
    {
        [Test, MoqAutoData]
        public async Task Then_GetsApprenticeFromMediator(
                GetApprenticeTrainingProvidersQuery request,
                GetApprenticeTrainingProvidersResult mediatorResult,
                [Frozen] Mock<IMediator> mockMediator,
                [Greedy] ProviderController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetApprenticeTrainingProvidersQuery>(x => x.ApprenticeId == request.ApprenticeId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetApprenticeTrainingProviders(request);

            Assert.That(controllerResult, Is.Not.Null);
            var model = controllerResult.Value;

            Assert.That(model, Is.Not.Null);
            model.Should().BeEquivalentTo(mediatorResult);
        }
    }
}
