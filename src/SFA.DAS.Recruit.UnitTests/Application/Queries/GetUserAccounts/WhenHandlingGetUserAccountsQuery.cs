using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.Recruit.Application.Queries.GetUserAccounts;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Recruit.UnitTests.Application.Queries.GetUserAccounts
{
    public class WhenHandlingGetUserAccountsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Request_Is_Handled_And_Data_Returned(
            GetUserAccountsQuery query,
            List<GetAccountsByUserResponse> apiResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApiClient,
            GetUserAccountsQueryHandler handler)
        {
            //Arrange
            var expectedGetRequest = new GetAccountsByUserRequest(query.UserId);
            accountApiClient.Setup(x =>
                x.GetAll<GetAccountsByUserResponse>(
                    It.Is<GetAccountsByUserRequest>(c=>c.GetAllUrl.Equals(expectedGetRequest.GetAllUrl))))
                .ReturnsAsync(apiResponse);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.HashedAccountIds.Should().BeEquivalentTo(apiResponse.Select(c=>c.HashedAccountId).ToList());
        }

        [Test, MoqAutoData]
        public async Task Then_If_NotFound_Response_Then_Null_Returned(
            GetUserAccountsQuery query,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountApiClient,
            GetUserAccountsQueryHandler handler)
        {
            //Arrange
            accountApiClient.Setup(x =>
                    x.GetAll<GetAccountsByUserResponse>(
                        It.IsAny<GetAccountsByUserRequest>()))
                .ReturnsAsync((List<GetAccountsByUserResponse>)null);
            
            //Act
            var actual = await handler.Handle(query, CancellationToken.None);
            
            //Assert
            actual.HashedAccountIds.Should().BeNullOrEmpty();
        }
    }
}