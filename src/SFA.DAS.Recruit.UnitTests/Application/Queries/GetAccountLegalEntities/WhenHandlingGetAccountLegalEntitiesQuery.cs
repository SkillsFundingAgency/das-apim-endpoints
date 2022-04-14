using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAccountLegalEntities
{
    public class WhenHandlingGetAccountLegalEntitiesQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetAccountLegalEntitiesQuery query,
            List<GetAccountLegalEntityResponseItem> apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApiClient,
            GetAccountLegalEntitiesQueryHandler handler)
        {
            //Arrange
            var expectedGet = new GetAccountLegalEntitiesRequest(query.HashedAccountId);
            accountApiClient
                .Setup(x => x.GetAll<GetAccountLegalEntityResponseItem>(
                    It.Is<GetAccountLegalEntitiesRequest>(c => c.GetAllUrl.Equals(expectedGet.GetAllUrl))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.AccountLegalEntities.Should().BeEquivalentTo(apiResponse);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Not_Found_Then_Empty_List_Returned(
            GetAccountLegalEntitiesQuery query,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApiClient,
            GetAccountLegalEntitiesQueryHandler handler)
        {
            //Arrange
            accountApiClient
                .Setup(x => x.GetAll<GetAccountLegalEntityResponseItem>(
                    It.IsAny<GetAccountLegalEntitiesRequest>()))
                .ReturnsAsync((List<GetAccountLegalEntityResponseItem>)null);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.AccountLegalEntities.Should().BeNullOrEmpty();
        }
    }
}