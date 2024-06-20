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