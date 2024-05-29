using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.Cohorts.Queries.GetCohort;
using SFA.DAS.Reservations.Application.Providers.Queries.GetCohort;
using SFA.DAS.Reservations.InnerApi.Requests;
using SFA.DAS.Reservations.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.Cohorts.Queries;

public class WhenGettingCohort
{
    [Test, MoqAutoData]
    public async Task Then_Gets_Cohort_From_CommitmentsV2_Api(
        GetCohortQuery query,
        GetCohortResponse apiResponse,
        [Frozen] Mock<ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration>> mockApiClient,
        GetCohortQueryHandler handler)
    {
        mockApiClient
            .Setup(client => client.Get<GetCohortResponse>(It.Is<GetCohortRequest>(request => request.CohortId == query.CohortId)))
            .ReturnsAsync(apiResponse);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Cohort.Should().BeEquivalentTo(apiResponse);
    }
}