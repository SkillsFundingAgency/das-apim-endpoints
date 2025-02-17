using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Interfaces;

public interface IAccountsService
{
    Task<Account> GetAccount(long accountId);
    Task<LegalEntity> GetEmployerAccountLegalEntity(string url);
    Task<List<Account>> GetUserAccounts(Guid userId);
    Task<List<InnerApi.Responses.TeamMember>> GetAccountTeamMembers(long accountId);
    Task<ApiResponse<SendInvitationRequest>> SendInvitation(SendInvitationRequest request);
    Task<ApiResponse<ResendInvitationRequest>> ResendInvitation(ResendInvitationRequest request);
}