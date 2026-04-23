using System.Threading;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.VacanciesManage.Application.Providers.Queries.GetProviderAccountLegalEntities;

namespace SFA.DAS.VacanciesManage.UnitTests.Application.Providers.Queries
{
    public class WhenHandlingGetProviderAccountLegalEntitiesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Api_Called_For_Ukprn(
            GetProviderAccountLegalEntitiesResponse apiQueryResponse,
            GetProviderAccountLegalEntitiesQuery query,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> apiClient,
            GetProviderAccountLegalEntitiesQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.Get<GetProviderAccountLegalEntitiesResponse>(
                        It.Is<GetProviderAccountLegalEntitiesRequest>(c => c.GetUrl.Contains($"ukprn={query.Ukprn}"))))
                .ReturnsAsync(apiQueryResponse);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.ProviderAccountLegalEntities.Should().BeEquivalentTo(apiQueryResponse.AccountProviderLegalEntities);
        }
    }
}