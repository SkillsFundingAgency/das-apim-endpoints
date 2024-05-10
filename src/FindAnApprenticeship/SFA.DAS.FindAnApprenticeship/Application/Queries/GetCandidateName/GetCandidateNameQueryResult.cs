using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateName;
public class GetCandidateNameQueryResult
{
    public string FirstName { get; set; }
    public string MiddleNames { get; set; }
    public string LastName { get; set; }

    public static implicit operator GetCandidateNameQueryResult(GetCandidateApiResponse source)
    {
        return new GetCandidateNameQueryResult
        {
            FirstName = source?.FirstName,
            MiddleNames = source?.MiddleNames,
            LastName = source?.LastName
        };
    }
}
