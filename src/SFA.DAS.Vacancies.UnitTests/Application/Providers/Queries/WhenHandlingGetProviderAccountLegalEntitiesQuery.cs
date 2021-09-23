using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.UnitTests.Application.Providers.Queries
{
    public class WhenHandlingGetProviderAccountLegalEntitiesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Api_Called_For_Ukprn(
            GetProviderAccountLegalEntitiesResponse apiResponse,
            GetProviderAccountLegalEntitiesQuery query,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> apiClient,
            GetProviderAccountLegalEntitiesQueryHandler handler)
        {
            apiClient.Setup(x =>
                    x.Get<GetProviderAccountLegalEntitiesResponse>(
                        It.Is<GetProviderAccountLegalEntitiesRequest>(c => c.GetUrl.Contains($"ukprn={query.Ukprn}"))))
                .ReturnsAsync(apiResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);

            actual.ProviderAccountLegalEntities.Should().BeEquivalentTo(apiResponse.ProviderAccountLegalEntities);
        }
    }
}