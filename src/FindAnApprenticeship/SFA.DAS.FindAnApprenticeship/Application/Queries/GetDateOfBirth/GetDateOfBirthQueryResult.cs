using System;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetDateOfBirth;
public class GetDateOfBirthQueryResult
{
    public DateTime? DateOfBirth { get; set; }

    public static implicit operator GetDateOfBirthQueryResult(GetCandidateApiResponse source)
    {
        return new GetDateOfBirthQueryResult
        {
            DateOfBirth = source?.DateOfBirth
        };
    }
}
