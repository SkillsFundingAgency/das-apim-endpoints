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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.GetEmployerAccountTaskList;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.User;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAgreements;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.GetEmployerAccountTaskList;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.User;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCreateAccountTaskList;

public class WhenHandlingGetCreateAccountTaskListQuery
{
    [Test, MoqAutoData]
    public async Task When_HashedAccountId_Is_Null_And_No_Accounts_Returned_From_InnerApi_Then_Appropriate_Response_Is_Created(
        long accountId,
        string userRef,
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{userRef}"))))
            .ReturnsAsync(userResponse);

        var query = new GetCreateAccountTaskListQuery(accountId, null, userRef);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.UserFirstName.Should().Be(userResponse.FirstName);
        actual.UserLastName.Should().Be(userResponse.LastName);
        actual.HashedAccountId.Should().BeNullOrEmpty();
        actual.HasPayeScheme.Should().BeFalse();
        actual.NameConfirmed.Should().BeFalse();

        accountsApiClient
            .Verify(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{userRef}"))), Times.Once);

        accountsApiClient
            .Verify(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.IsAny<GetAccountPayeSchemesRequest>()), Times.Never);
    }

    [Test, MoqAutoData]
    public async Task When_HashedAccountId_Is_Null_And_Accounts_Returned_From_InnerApi_Then_Response_Is_Created_From_Most_Recent_Account(
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
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{userRef}"))))
            .ReturnsAsync(userResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetUserAccountsResponse>(It.Is<GetUserAccountsRequest>(c => c.GetAllUrl.Equals($"api/user/{userRef}/accounts"))))
            .ReturnsAsync(accountsResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.Is<GetAccountPayeSchemesRequest>(c => c.GetAllUrl.Equals($"api/accounts/{firstAccount.EncodedAccountId}/payeschemes"))))
            .ReturnsAsync(payeSchemesResponse);

        var query = new GetCreateAccountTaskListQuery(accountId, null, userRef);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().NotBeNull();
        actual.UserFirstName.Should().Be(userResponse.FirstName);
        actual.UserLastName.Should().Be(userResponse.LastName);

        actual.HashedAccountId.Should().Be(firstAccount.EncodedAccountId);
        actual.HasPayeScheme.Should().Be(payeSchemesResponse.Count > 0);
        actual.NameConfirmed.Should().Be(firstAccount.NameConfirmed);

        accountsApiClient
            .Verify(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{userRef}"))
                ), Times.Once);

        accountsApiClient
            .Verify(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.Is<GetAccountPayeSchemesRequest>(c => c.GetAllUrl.Equals($"api/accounts/{firstAccount.EncodedAccountId}/payeschemes"))
                ), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task When_HashedAccountId_Is_Not_Null_And_Account_Is_Null_Then_Null_Response_Is_Returned(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQuery query,
        List<GetAccountPayeSchemesResponse> payeSchemesResponse,
        GetEmployerAccountTaskListResponse taskListResponse,
        //GetAccountByHashedIdResponse accountResponse,
        List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}"))))
            .ReturnsAsync(userResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetEmployerAccountTaskListResponse>(It.Is<GetEmployerAccountTaskListRequest>(c => c.GetUrl.Equals($"accounts/{query.AccountId}/account-task-list?hashedAccountId={query.HashedAccountId}"))))
            .ReturnsAsync(taskListResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByHashedIdResponse>(It.Is<GetAccountByHashedIdRequest>(c => c.GetUrl.Equals($"api/accounts/{query.HashedAccountId}"))))
            .ReturnsAsync(() => null);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c => c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync(employerAgreementsResponse);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().BeNull();

        accountsApiClient
            .Verify(x =>
                    x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}")))
                , Times.Once());

        accountsApiClient
            .Verify(x =>
                    x.Get<GetEmployerAccountTaskListResponse>(It.Is<GetEmployerAccountTaskListRequest>(c => c.GetUrl.Equals($"accounts/{query.AccountId}/account-task-list?hashedAccountId={query.HashedAccountId}")))
                , Times.Once());

        accountsApiClient
            .Verify(x =>
                    x.Get<GetAccountByHashedIdResponse>(It.Is<GetAccountByHashedIdRequest>(c => c.GetUrl.Equals($"api/accounts/{query.HashedAccountId}")))
                , Times.Once());

        accountsApiClient
            .Verify(x =>
                    x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c => c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements")))
                , Times.Once());
    }

    [Test, MoqAutoData]
    public async Task When_HashedAccountId_Is_Not_Null_And_There_Are_No_Agreements_Then_Null_Response_Is_Returned(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetUserByRefResponse userResponse,
        GetCreateAccountTaskListQuery query,
        List<GetAccountPayeSchemesResponse> payeSchemesResponse,
        GetEmployerAccountTaskListResponse taskListResponse,
        GetAccountByHashedIdResponse accountResponse,
        List<GetEmployerAgreementsResponse> employerAgreementsResponse,
        GetCreateAccountTaskListQueryHandler sut
    )
    {
        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}"))))
            .ReturnsAsync(userResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetEmployerAccountTaskListResponse>(It.Is<GetEmployerAccountTaskListRequest>(c => c.GetUrl.Equals($"accounts/{query.AccountId}/account-task-list?hashedAccountId={query.HashedAccountId}"))))
            .ReturnsAsync(taskListResponse);

        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByHashedIdResponse>(It.Is<GetAccountByHashedIdRequest>(c => c.GetUrl.Equals($"api/accounts/{query.HashedAccountId}"))))
            .ReturnsAsync(accountResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c => c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements"))))
            .ReturnsAsync([]);

        var actual = await sut.Handle(query, CancellationToken.None);

        actual.Should().BeNull();

        accountsApiClient
            .Verify(x =>
                    x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c => c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}")))
                , Times.Once());

        accountsApiClient
            .Verify(x =>
                    x.Get<GetEmployerAccountTaskListResponse>(It.Is<GetEmployerAccountTaskListRequest>(c => c.GetUrl.Equals($"accounts/{query.AccountId}/account-task-list?hashedAccountId={query.HashedAccountId}")))
                , Times.Once());

        accountsApiClient
            .Verify(x =>
                    x.Get<GetAccountByHashedIdResponse>(It.Is<GetAccountByHashedIdRequest>(c => c.GetUrl.Equals($"api/accounts/{query.HashedAccountId}")))
                , Times.Once());

        accountsApiClient
            .Verify(x =>
                    x.GetAll<GetEmployerAgreementsResponse>(It.Is<GetEmployerAgreementsRequest>(c => c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/agreements")))
                , Times.Once());
    }
}