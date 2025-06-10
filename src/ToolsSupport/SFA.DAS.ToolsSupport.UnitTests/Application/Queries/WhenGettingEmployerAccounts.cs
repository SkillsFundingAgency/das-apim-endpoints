using AutoFixture.NUnit3;
using Moq;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using FluentAssertions;
using System.Net;
using SFA.DAS.ToolsSupport.Application.Queries.SearchEmployerAccounts;

namespace SFA.DAS.ToolsSupport.UnitTests.Application.Queries;

public class WhenGettingEmployerAccounts
{
    [Test, MoqAutoData]
    public async Task Then_Gets_SingleAccount_With_Matching_PublicHashedId(
        long accountId,
        GetEmployerAccountByIdResponse mockApiResponse,
        [Frozen] Mock<IInternalApiClient<AccountsConfiguration>> mockApiClient,
        SearchEmployerAccountsQueryHandler sut)
    {
        var mockQuery = new SearchEmployerAccountsQuery {AccountId = accountId, PayeSchemeRef = null};
        var expectedUrl = $"api/accounts/{mockQuery.AccountId}";
        mockApiClient.Setup(client => client.Get<GetEmployerAccountByIdResponse>(It.Is<GetEmployerAccountByIdRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync(mockApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.EmployerAccounts.Count.Should().Be(1);
        actual.EmployerAccounts[0].Should().BeEquivalentTo(mockApiResponse);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_No_Account_With_Matching_PublicHashedId(
        long accountId,
        [Frozen] Mock<IInternalApiClient<AccountsConfiguration>> mockApiClient,
        SearchEmployerAccountsQueryHandler sut)
    {
        var mockQuery = new SearchEmployerAccountsQuery { AccountId = accountId, PayeSchemeRef = null };
        var expectedUrl = $"api/accounts/{mockQuery.AccountId}";
        mockApiClient.Setup(client => client.Get<GetEmployerAccountByIdResponse>(It.Is<GetEmployerAccountByIdRequest>(c => c.GetUrl == expectedUrl)))
            .ReturnsAsync((GetEmployerAccountByIdResponse)null!);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.EmployerAccounts.Count.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_None_When_No_Matching_PayeSchemeRef_Found(
        string payeSchemeRef,
        GetEmployerAccountByPayeResponse mockPayeApiResponse,
        GetEmployerAccountByIdResponse mockAccountApiResponse,
        [Frozen] Mock<IInternalApiClient<AccountsConfiguration>> mockApiClient,
        SearchEmployerAccountsQueryHandler sut)
    {
        var mockQuery = new SearchEmployerAccountsQuery { AccountId = null, PayeSchemeRef = payeSchemeRef };
        var expectedPayeUrl = $"api/accounthistories?payeRef={WebUtility.UrlEncode(payeSchemeRef)}";
        var expectedAccountUrl = $"api/accounts/{mockPayeApiResponse.AccountId}";
        mockApiClient.Setup(client => client.Get<GetEmployerAccountByPayeResponse>(It.Is<GetEmployerAccountByPayeRequest>(c => c.GetUrl == expectedPayeUrl)))
            .ReturnsAsync((GetEmployerAccountByPayeResponse)null!);
        mockApiClient.Setup(client => client.Get<GetEmployerAccountByIdResponse>(It.Is<GetEmployerAccountByIdRequest>(c => c.GetUrl == expectedAccountUrl)))
            .ReturnsAsync(mockAccountApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.EmployerAccounts.Count.Should().Be(0);
    }

    [Test, MoqAutoData]
    public async Task Then_Gets_SingleAccount_With_Matching_PayeSchemeRef(
        string payeSchemeRef,
        GetEmployerAccountByPayeResponse mockPayeApiResponse,
        GetEmployerAccountByIdResponse mockAccountApiResponse,
        [Frozen] Mock<IInternalApiClient<AccountsConfiguration>> mockApiClient,
        SearchEmployerAccountsQueryHandler sut)
    {
        var mockQuery = new SearchEmployerAccountsQuery { AccountId = null, PayeSchemeRef = payeSchemeRef };
        var expectedPayeUrl = $"api/accounthistories?payeRef={WebUtility.UrlEncode(payeSchemeRef)}";
        var expectedAccountUrl = $"api/accounts/{mockPayeApiResponse.AccountId}";
        mockApiClient.Setup(client => client.Get<GetEmployerAccountByPayeResponse>(It.Is<GetEmployerAccountByPayeRequest>(c => c.GetUrl == expectedPayeUrl)))
            .ReturnsAsync(mockPayeApiResponse);
        mockApiClient.Setup(client => client.Get<GetEmployerAccountByIdResponse>(It.Is<GetEmployerAccountByIdRequest>(c => c.GetUrl == expectedAccountUrl)))
            .ReturnsAsync(mockAccountApiResponse);

        var actual = await sut.Handle(mockQuery, It.IsAny<CancellationToken>());

        actual.EmployerAccounts.Count.Should().Be(1);
        actual.EmployerAccounts[0].Should().BeEquivalentTo(mockAccountApiResponse);
    }
}