namespace SFA.DAS.Apprenticeships.Responses;

public class GetAccountLegalEntityResponse
{
	public long AccountId { get; set; }
	public long MaLegalEntityId { get; set; }
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
	public string AccountName { get; set; }
	public string LegalEntityName { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
}