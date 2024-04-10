using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.PhoneNumber;

public class GetPhoneNumberQueryResult
{
    public string PhoneNumber { get; set; }

    public static implicit operator GetPhoneNumberQueryResult(GetCandidateApiResponse source)
    {
        return new GetPhoneNumberQueryResult
        {
            PhoneNumber = source?.PhoneNumber
        };
    }
}