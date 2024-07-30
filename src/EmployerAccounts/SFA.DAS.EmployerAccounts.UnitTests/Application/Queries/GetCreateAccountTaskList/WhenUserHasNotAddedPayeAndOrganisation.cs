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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.User;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PayeSchemes;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.User;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.Queries.GetCreateAccountTaskList;

public class WhenUserHasNotAddedPayeAndOrganisation
{
    [Test, MoqAutoData]
    public async Task Then_GetUserAccounts(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetUserByRefResponse userResponse,
        GetAccountByIdResponse accountResponse,
        GetCreateAccountTaskListQueryHandler sut,
        long accountId,
        string userRef
    )
    {
        var query = new GetCreateAccountTaskListQuery(accountId, null, userRef);

        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c =>
                    c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}"))))
            .ReturnsAsync(userResponse)
            .Verifiable();

        await sut.Handle(query, CancellationToken.None);

        accountsApiClient.Verify();
    }

    [Test, MoqAutoData]
    public async Task Then_GetPaye_Schemes(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetUserByRefResponse userResponse,
        GetAccountByIdResponse accountResponse,
        GetCreateAccountTaskListQueryHandler sut,
        long accountId,
        string userRef
    )
    {
        var query = new GetCreateAccountTaskListQuery(accountId, null, userRef);
        
        accountsApiClient
            .Setup(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.Is<GetAccountPayeSchemesRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{query.AccountId}/payeschemes"))))
            .ReturnsAsync(() => null);

        await sut.Handle(query, CancellationToken.None);

        accountsApiClient.Verify();
    }

    [Test, MoqAutoData]
    public async Task Then_HasPaye_False(
        [Frozen] Mock<IAccountsApiClient<AccountsConfiguration>> accountsApiClient,
        GetUserByRefResponse userResponse,
        GetAccountByIdResponse accountResponse,
        List<GetUserAccountsResponse> userAccountsResponse,
        GetUserAccountsRequest userAccountsRequest,
        GetCreateAccountTaskListQueryHandler sut,
        long accountId,
        string userRef
    )
    {
        var query = new GetCreateAccountTaskListQuery(accountId, null, userRef);

        accountsApiClient
            .Setup(x =>
                x.Get<GetUserByRefResponse>(It.Is<GetUserByRefRequest>(c =>
                    c.GetUrl.Equals($"api/user/by-ref/{query.UserRef}"))))
            .ReturnsAsync(userResponse);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetUserAccountsResponse>(It.Is<GetUserAccountsRequest>(c =>
                    c.GetAllUrl.Equals($"api/user/{query.UserRef}/accounts"))))
            .ReturnsAsync(userAccountsResponse)
            .Verifiable();

        accountsApiClient
            .Setup(x =>
                x.Get<GetAccountByIdResponse>(It.Is<GetAccountByIdRequest>(c =>
                    c.GetUrl.Equals($"api/accounts/{query.AccountId}"))))
            .ReturnsAsync(accountResponse);

        var firstAccount = userAccountsResponse.MinBy(x => x.DateRegistered);

        accountsApiClient
            .Setup(x =>
                x.GetAll<GetAccountPayeSchemesResponse>(It.Is<GetAccountPayeSchemesRequest>(c =>
                    c.GetAllUrl.Equals($"api/accounts/{firstAccount.AccountId}/payeschemes"))))
            .ReturnsAsync(() => null)
            .Verifiable();

        var result = await sut.Handle(query, CancellationToken.None);

        result.HasPayeScheme.Should().BeFalse();
        result.CompletedSections.Should().Be(1);

        accountsApiClient.Verify();
    }
}