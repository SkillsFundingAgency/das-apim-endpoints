using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderPermissions;

public enum Operation : short
{
    CreateCohort = 0,
    Recruitment = 1,
    RecruitmentRequiresReview = 2
}

public class GetHasPermissionRequest(long? ukPrn, long? accountLegalEntityId, Operation operation) : IGetApiRequest
{
    public string GetUrl => $"permissions/has?ukprn={ukPrn}&accountLegalEntityId={accountLegalEntityId}&operation={(int)operation}";
}