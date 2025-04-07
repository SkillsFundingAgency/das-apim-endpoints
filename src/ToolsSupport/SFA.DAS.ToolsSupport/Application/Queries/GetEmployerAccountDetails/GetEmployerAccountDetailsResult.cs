using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccountDetails;

public class GetEmployerAccountDetailsResult
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; } = "";
    public string PublicHashedAccountId { get; set; } = "";
    public string DasAccountName { get; set; } = "";
    public DateTime DateRegistered { get; set; }
    public string OwnerEmail { get; set; } = "";
    public decimal Balance { get; set; }
    public string ApprenticeshipEmployerType { get; set; } = "";

    public IEnumerable<LegalEntity> LegalEntities { get; set; } = [];
    public ICollection<TeamMember> TeamMembers { get; set; } = [];
    public IEnumerable<PayeScheme> PayeSchemes { get; set; } = [];
    public IEnumerable<Transaction> Transactions { get; set; } = [];


}
