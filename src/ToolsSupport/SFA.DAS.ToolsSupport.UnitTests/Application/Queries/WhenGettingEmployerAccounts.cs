using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.Application.Queries;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using FluentAssertions;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;

public class WhenGettingEmployerAccounts
{
    [Test, MoqAutoData]
    public async Task Then_Gets_SingleAccount_With_Matching_PublicHashedId(
        long accountId,
        GetEmployerAccountByIdResponse mockApiResponse,
        [Frozen] Mock<IInternalApiClient<AccountsConfiguration>> mockApiClient,
        GetEmployerAccountsQueryHandler sut)
    {
        var mockQuery = new GetEmployerAccountsQuery {AccountId = accountId, PayeSchemeRef = null};
        var expectedUrl = $"api/accounts/{mockQuery.AccountId}";
        mockApiClient.Setup(client => client.Get<GetEmployerAccountByIdResponse>(It.Is<GetEmployerAccountByIdRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync(mockApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Accounts.Count.Should().Be(1);
        actual.Accounts[0].Should().BeEquivalentTo(mockApiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_No_Account_With_Matching_PublicHashedId(
        long accountId,
        [Frozen] Mock<IInternalApiClient<AccountsConfiguration>> mockApiClient,
        GetEmployerAccountsQueryHandler sut)
    {
        var mockQuery = new GetEmployerAccountsQuery { AccountId = accountId, PayeSchemeRef = null };
        var expectedUrl = $"api/accounts/{mockQuery.AccountId}";
        mockApiClient.Setup(client => client.Get<GetEmployerAccountByIdResponse>(It.Is<GetEmployerAccountByIdRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync((GetEmployerAccountByIdResponse)null!);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.Accounts.Count.Should().Be(0);
    }

}