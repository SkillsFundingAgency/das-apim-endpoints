using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.ProviderAccounts.Queries;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.ProviderAccounts;

public class WhenHandlingGetProviderAccountLegalEntitiesWithCreatCohortQuery
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Api_Called_For_Ukprn(
        GetProviderAccountLegalEntitiesWithCreateCohortResult apiQueryResponse,
        GetProviderAccountLegalEntitiesWithCreatCohortQuery withCreatCohortQuery,
        [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> apiClient,
        GetProviderAccountLegalEntitiesWithCreatCohortQueryHandler handler)
    {
        apiClient.Setup(x =>
                x.Get<GetProviderAccountLegalEntitiesWithCreateCohortResult>(
                    It.Is<GetProviderAccountLegalEntitiesRequest>(c => c.GetUrl.Contains($"ukprn={withCreatCohortQuery.Ukprn}&operations={(int)Operation.CreateCohort}"))))
            .ReturnsAsync(apiQueryResponse);

        var actual = await handler.Handle(withCreatCohortQuery, CancellationToken.None);

        actual.AccountProviderLegalEntities.Should().BeEquivalentTo(apiQueryResponse.AccountProviderLegalEntities);
    }
}