using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.UnitTests.Application.Providers.Queries
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