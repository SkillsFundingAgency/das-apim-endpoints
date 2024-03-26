using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetCandidateAddress;
public class GetCandidatePostcodeQueryResult
{
    public string? Postcode { get; set; }

    public static implicit operator GetCandidatePostcodeQueryResult(GetCandidatePostcodeApiResponse source)
    {
        return new GetCandidatePostcodeQueryResult
        {
            Postcode = source.Postcode
        };
    }
}
