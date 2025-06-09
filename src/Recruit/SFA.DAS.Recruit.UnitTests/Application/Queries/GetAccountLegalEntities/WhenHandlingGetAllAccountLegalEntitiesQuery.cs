using SFA.DAS.Recruit.Application.Queries.GetAllAccountLegalEntities;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
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
            var expectedGet = new GetAllAccountLegalEntitiesApiRequest(new GetAllAccountLegalEntitiesApiRequest.GetAllAccountLegalEntitiesApiRequestData
            {
                AccountIds = query.AccountIds,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                SearchTerm = query.SearchTerm,
                SortColumn = query.SortColumn,
                IsAscending = query.IsAscending
            });

            accountApiClient
                .Setup(x => x.PostWithResponseCode<GetAllAccountLegalEntitiesApiResponse>(
                    It.Is<GetAllAccountLegalEntitiesApiRequest>(c => c.PostUrl.Equals(expectedGet.PostUrl)), true))
                .ReturnsAsync(new ApiResponse<GetAllAccountLegalEntitiesApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            //Act
            var actual = await handler.Handle(query, CancellationToken.None);

            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
        }
    }
}