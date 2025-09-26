using SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities;
using SFA.DAS.Recruit.InnerApi.Responses;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAccountLegalEntities
{
    [TestFixture]
    public class WhenMappingApiResponseWithGetAllAccountLegalEntitiesResult
    {
        [Test, MoqAutoData]
        public void ImplicitOperator_MapsApiResponseToQueryResult(
            GetAllAccountLegalEntitiesApiResponse response)
        {
            // Act
            GetAllAccountLegalEntitiesQueryResult result = response;

            // Assert
            result.PageInfo.Should().Be(response.PageInfo);
            result.LegalEntities.Should().BeEquivalentTo(response.LegalEntities);
        }
    }
}