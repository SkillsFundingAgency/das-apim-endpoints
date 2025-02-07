using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.EmployerAccount.GetEmployerAccountDetails;

public class GetEmployerAccountDetailsResult
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; } = "";
    public string DasAccountName { get; set; } = "";
    public DateTime DateRegistered { get; set; }
    public string OwnerEmail { get; set; } = "";
    public IEnumerable<LegalEntity> LegalEntities { get; set; } = [];
    public string ApprenticeshipEmployerType { get; set; } = "";


    //public IEnumerable<PayeSchemeViewModel> PayeSchemes { get; set; }
    //public ICollection<TeamMemberViewModel> TeamMembers { get; set; }
    //public IEnumerable<TransactionViewModel> Transactions { get; set; }
}
