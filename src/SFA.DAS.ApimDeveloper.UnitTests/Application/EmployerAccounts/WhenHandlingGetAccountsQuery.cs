using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Application.EmployerAccounts.Queries;
using SFA.DAS.ApimDeveloper.InnerApi.Requests;
using SFA.DAS.ApimDeveloper.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApimDeveloper.UnitTests.Application.EmployerAccounts
{
    public class WhenHandlingGetAccountsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Is_Handled_And_Data_Returned(
            GetAccountsQuery query,
            List<GetUserAccountsResponse> apiResponse,
            GetAccountTeamMembersResponse teamResponse,
            [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
            GetAccountsQueryHandler handler)
        {
            teamResponse.UserRef = query.UserId;
            accountsApiClient
                .Setup(x => x.GetAll<GetUserAccountsResponse>(
                    It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Contains($"user/{query.UserId}/accounts"))))
                .ReturnsAsync(apiResponse);
            accountsApiClient
                .Setup(x => x.GetAll<GetAccountTeamMembersResponse>(
                    It.Is<GetAccountTeamMembersRequest>(c => c.GetAllUrl.Contains($"accounts/{apiResponse.First().EncodedAccountId}/users"))))
                .ReturnsAsync(new List<GetAccountTeamMembersResponse>{teamResponse});

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.UserAccountResponse.First().Role.Should().Be(teamResponse.Role);
            actual.UserAccountResponse.First().DasAccountName.Should().Be(apiResponse.First().DasAccountName);
            actual.UserAccountResponse.First().EncodedAccountId.Should().Be(apiResponse.First().EncodedAccountId);
        }
    }
}