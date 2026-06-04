namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.Ukrlp;

public class GetUkrlpDataQueryResponse
{
    public bool Success { get; set; }
    public List<ProviderAddress> Results { get; set; }
}