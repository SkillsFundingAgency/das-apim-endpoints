using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Providers.GetRoatpProviders;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RoatpV2;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Application.Providers.Queries;

public class WhenGettingProviders
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Providers_From_RoatpTrainingProviderService(
        [Frozen] Mock<IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration>> apiClient,
        [Greedy] GetRoatpProvidersQueryHandler handler,
        GetProvidersResponse expected,
        CancellationToken cancellationToken
    )
    {
        var query = new GetRoatpProvidersQuery();

        apiClient.Setup(x =>
            x.GetWithResponseCode<GetProvidersResponse>(
                It.IsAny<GetRoatpProvidersRequest>()
            )
        )
        .ReturnsAsync(new ApiResponse<GetProvidersResponse>(expected, HttpStatusCode.OK, string.Empty));

        var _sut = await handler.Handle(query, cancellationToken);

        _sut.Providers.Should().BeEquivalentTo(expected.RegisteredProviders.Select(provider => (RoatpProvider)provider));
    }
}
