namespace SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;
public class GetEasUserByEmailQueryResult
{
    public bool HasUserAccount { get; set; }
    public bool HasOneEmployerAccount { get; set; }
    public long? AccountId { get; set; }
    public bool HasOneLegalEntity { get; set; }
}
