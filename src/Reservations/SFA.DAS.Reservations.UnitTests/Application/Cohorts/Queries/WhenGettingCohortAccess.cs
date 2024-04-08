using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.Cohorts.Queries.GetCohortAccess;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.Cohorts.Queries;

public class WhenGettingCohortAccess
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Permission_From_CommitmentsV2_Api(
        GetCohortAccessQuery query,
        bool apiResponse,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetCohortAccessQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<bool>(It.Is<GetCohortAccessRequest>(request => request.CohortId == query.CohortId && request.ProviderId == query.ProviderId)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Should().Be(apiResponse);
    }
}