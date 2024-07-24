namespace SFA.DAS.ProviderPR.Application.Queries.GetEasUserByEmail;

public record GetEasUserByEmailQueryResult(bool HasUserAccount, bool HasOneEmployerAccount, long? AccountId, bool HasOneLegalEntity);