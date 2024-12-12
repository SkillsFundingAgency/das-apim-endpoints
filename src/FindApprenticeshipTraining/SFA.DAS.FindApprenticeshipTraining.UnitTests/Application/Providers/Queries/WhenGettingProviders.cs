using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Providers.Queries;
public class WhenGettingProviders
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Providers_From_RoatpTrainingProviderService(
        [Frozen] Mock<IRoatpV2TrainingProviderService> trainingProviderService,
        [Greedy] GetRoatpProvidersQueryHandler handler,
        GetProvidersResponse expected,
        CancellationToken cancellationToken)
    {
        var query = new GetRoatpProvidersQuery();
        trainingProviderService.Setup(x => x.GetProviders(cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Providers.Should().BeEquivalentTo(expected.RegisteredProviders.Select(provider => (RoatpProvider)provider));
    }
}
