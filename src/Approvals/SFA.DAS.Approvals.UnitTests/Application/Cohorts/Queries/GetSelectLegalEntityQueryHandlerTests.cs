using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectLegalEntity;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.UnitTests.Application.Cohorts.Queries;

public class GetSelectLegalEntityQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Is_Handled_And_Data_Returned(
        GetSelectLegalEntityQuery query,
        List<GetLegalEntitiesForAccountResponseItem> apiResponse,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApiClient,
        GetSelectLegalEntityQueryHandler handler)
    {
        //Arrange
        var expectedGet = new GetAccountLegalEntitiesRequest(query.AccountId);
        accountApiClient
            .Setup(x => x.GetAll<GetLegalEntitiesForAccountResponseItem>(
                It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Equals(expectedGet.GetAllUrl))))
            .ReturnsAsync(apiResponse);
            
        //Act
        var actual = await handler.Handle(query, CancellationToken.None);
            
        //Assert
        actual.LegalEntities.Should().BeEquivalentTo(apiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_If_Not_Found_Then_Empty_List_Returned(
        GetSelectLegalEntityQuery query,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApiClient,
        GetSelectLegalEntityQueryHandler handler)
    {
        //Arrange
        accountApiClient
            .Setup(x => x.GetAll<GetLegalEntitiesForAccountResponseItem>(
                It.IsAny<GetAccountLegalEntitiesRequest>()))
            .ReturnsAsync((List<GetLegalEntitiesForAccountResponseItem>)null);
            
        //Act
        var actual = await handler.Handle(query, CancellationToken.None);
            
        //Assert
        actual.LegalEntities.Should().BeNullOrEmpty();
    }
}