using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;
using SFA.DAS.ToolsSupport.Interfaces;

namespace SFA.DAS.ToolsSupport.Application.Services;

public class AccountsService(IAccountsApiClient<AccountsConfiguration> client) : IAccountsService
{
    public async Task<Account> GetAccount(long accountId)
    {
        var response = await client.Get<Account>(new GetEmployerAccountByIdRequest(accountId));

        return response;
    }

    public async Task<LegalEntity> GetEmployerAccountLegalEntity(string url)
    {
        var response = await client.Get<LegalEntity>(new GetEmployerAccountLegalEntityRequest(url));

        return response;
    }

    public async Task<List<Account>> GetUserAccounts(Guid userId)
    {
        var response = await client.Get<List<Account>>(new GetUserAccountsRequest(userId));

        return response;
    }

    public async Task<List<InnerApi.Responses.TeamMember>> GetAccountTeamMembers(long accountId)
    {
        var response = await client.Get<List<InnerApi.Responses.TeamMember>>(new GetAccountTeamMembersRequest(accountId));

        return response;
    }

    public async Task<ApiResponse<SendInvitationRequest>> SendInvitation(SendInvitationRequest request)
    {
        return await client.PostWithResponseCode<SendInvitationRequest>(request, false);
    }

    public async Task<ApiResponse<ResendInvitationRequest>> ResendInvitation(ResendInvitationRequest request)
    {
        return await client.PostWithResponseCode<ResendInvitationRequest>(request, false);
    }
}
