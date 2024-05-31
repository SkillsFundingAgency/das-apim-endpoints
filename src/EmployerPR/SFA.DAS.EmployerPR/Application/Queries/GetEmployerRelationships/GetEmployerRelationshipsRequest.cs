using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerPR.Application.Queries.GetEmployerRelationships;

#nullable enable

public class GetEmployerRelationshipsRequest
{
    public string AccountHashedId { get; set; }

    public long? Ukprn { get; set; }

    public string? AccountlegalentityPublicHashedId { get; set; }

    public GetEmployerRelationshipsRequest(string accountHashedId, long? ukprn = null, string? accountlegalentityPublicHashedId = null)
    {
        AccountHashedId = accountHashedId;
        Ukprn = ukprn;
        AccountlegalentityPublicHashedId = accountlegalentityPublicHashedId;
    }
}
