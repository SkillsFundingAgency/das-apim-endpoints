using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Reservations.Application.AccountProviderLegalEntities.Queries;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Reservations.UnitTests.Application.AccountProviderLegalEntities.Queries
{
    public class WhenGettingAccountProviderLegalEntities
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_AccountProviderLegalEntities_From_ProviderRelationships_Api(
            GetAccountProviderLegalEntitiesQuery query,
            GetProviderAccountLegalEntitiesResponse apiResponse,
            [Frozen] Mock<IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration>> mockApiClient,
            GetAccountProviderLegalEntitiesQueryHandler handler)
        {
            mockApiClient
                .Setup(client => client.Get<GetProviderAccountLegalEntitiesResponse>(It.IsAny<GetProviderAccountLegalEntitiesRequest>()))
                .ReturnsAsync(apiResponse);

            var result = await handler.Handle(query, CancellationToken.None);

            result.AccountProviderLegalEntities.Should().BeEquivalentTo(apiResponse);
        }
    }
}
