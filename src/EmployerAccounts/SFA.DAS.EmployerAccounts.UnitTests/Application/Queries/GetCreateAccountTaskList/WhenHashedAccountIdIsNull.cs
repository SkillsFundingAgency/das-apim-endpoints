using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetCreateAccountTaskList;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.User;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.User;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCreateAccountTaskList;

public class WhenHashedAccountIdIsNull
{
    [Test, MoqAutoData]
    public async Task And_No_Accounts_Returned_From_InnerApi_Then_Null_Response_Is_Returned(
        long accountId,
        string userRef,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c =>
                    c.GetUrl.Equals($"api/user/by-ref/{userRef}"))))
            .ReturnsAsync(userResponse)
            .Verifiable();

        var query = new GetCreateAccountTaskListQuery(accountId, null, userRef);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().BeNull();

        accountsApiClient.Verify();

        accountsApiClient
            .Verify(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.IsAny<GetAccountPayeSchemesRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task And_Accounts_Returned_From_InnerApi_Then_Response_Is_Created_From_Most_Recent_Account(
        long accountId,
        string userRef,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetUserByRefResponse userResponse,
        List<GetUserAccountsResponse> accountsResponse,
        List<GetAccountPayeSchemesResponse> payeSchemesResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        var firstAccount = accountsResponse.MinBy(x => x.DateRegistered);

        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c =>
                    c.GetUrl.Equals($"api/user/by-ref/{userRef}"))))
            .ReturnsAsync(userResponse)
            .Verifiable();

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetUserAccountsResponse>(It.Is<GetUserAccountsRequest>(c =>
                    c.GetAllUrl.Equals($"api/user/{userRef}/accounts"))))
            .ReturnsAsync(accountsResponse)
            .Verifiable();

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.Is<GetAccountPayeSchemesRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{firstAccount.AccountId}/payeschemes"))))
            .ReturnsAsync(payeSchemesResponse)
            .Verifiable();

        var query = new GetCreateAccountTaskListQuery(accountId, null, userRef);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.UserFirstName.Should().Be(userResponse.FirstName);
        actual.UserLastName.Should().Be(userResponse.LastName);

        actual.HashedAccountId.Should().Be(firstAccount.EncodedAccountId);
        actual.HasPayeScheme.Should().Be(payeSchemesResponse.Count > 0);
        actual.NameConfirmed.Should().Be(firstAccount.NameConfirmed);

        accountsApiClient.Verify();
    }
}