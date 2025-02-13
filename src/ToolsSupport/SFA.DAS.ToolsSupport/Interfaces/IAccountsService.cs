using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Interfaces;

public interface IAccountsService
{
    Task<Account> GetAccount(long accountId);
    Task<LegalEntity> GetEmployerAccountLegalEntity(string url);
    Task<List<Account>> GetUserAccounts(Guid userId);
    Task<List<TeamMember>> GetAccountTeamMembers(long accountId);
}
