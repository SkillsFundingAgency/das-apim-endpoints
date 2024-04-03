using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;
public class GetCandidateNameQueryResult
{
    public string FirstName { get; set; }
    public string MiddleNames { get; set; }
    public string LastName { get; set; }

    public static implicit operator GetCandidateNameQueryResult(GetCandidateNameApiResponse source)
    {
        return new GetCandidateNameQueryResult
        {
            FirstName = source != null ? source.FirstName : null,
            MiddleNames = source != null ? source.MiddleNames : null,
            LastName = source != null ? source.LastName : null
        };
    }
}
