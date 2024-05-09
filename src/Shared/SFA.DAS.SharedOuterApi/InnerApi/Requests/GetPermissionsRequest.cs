using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;

public class GetPermissionsRequest : IGetApiRequest
{
    public string GetUrl => $"permissions?Ukprn={Ukprn}&PublicHashedId={PublicHashedId}";

    public long? Ukprn { get; }

    public string? PublicHashedId { get; set; }

    public GetPermissionsRequest(long? ukprn, string? publicHashedId)
    {
        Ukprn = ukprn;
        PublicHashedId = publicHashedId;
    }
}

public enum Operation : short
{
    CreateCohort = 0,
    Recruitment = 1,
    RecruitmentRequiresReview = 2
}