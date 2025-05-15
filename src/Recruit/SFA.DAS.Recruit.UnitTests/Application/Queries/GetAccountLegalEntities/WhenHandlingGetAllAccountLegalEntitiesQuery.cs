using SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAccountLegalEntities
{
    [TestFixture]
    public class WhenHandlingGetAllAccountLegalEntitiesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetAllAccountLegalEntitiesQuery query,
            GetAllAccountLegalEntitiesApiResponse apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApiClient,
            [Greedy] GetAllAccountLegalEntitiesQueryHandler handler)
        {
            //Arrange
            var expectedGet = new GetAllAccountLegalEntitiesApiRequest(
                query.AccountId,
                query.PageNumber,
                query.PageSize,
                query.SortColumn,
                query.IsAscending);

            accountApiClient
                .Setup(x => x.Get<GetAllAccountLegalEntitiesApiResponse>(
                    It.Is<GetAllAccountLegalEntitiesApiRequest>(c => c.GetUrl.Equals(expectedGet.GetUrl))))
                .ReturnsAsync(apiResponse);

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}