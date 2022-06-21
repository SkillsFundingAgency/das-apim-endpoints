using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetAccount;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetAccount
{
    public class WhenHandlingGetAccountQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetAccountQuery query,
            GetAccountByIdResponse apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            GetAccountQueryHandler handler)
        {
            //Arrange
            var expectedGetUrl = new GetAccountByIdRequest(query.AccountId);
            accountsApiClient
                .Setup(x => x.Get<GetAccountByIdResponse>(
                    It.Is<GetAccountByIdRequest>(c => c.GetUrl.Equals(expectedGetUrl.GetUrl))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.Should().BeEquivalentTo(apiResponse);
        }
        
        [Test, MoqAutoData]
        public async Task Then_If_NotFound_Response_Then_Null_Returned(
            GetAccountQuery query,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApiClient,
            GetAccountQueryHandler handler)
        {
            //Arrange
            accountApiClient.Setup(x =>
                    x.Get<GetAccountByIdResponse>(
                        It.IsAny<GetAccountByIdRequest>()))
                .ReturnsAsync((GetAccountByIdResponse)null);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.AccountId.Should().BeNull();
            actual.HashedAccountId.Should().BeNull();
        }
    }
}