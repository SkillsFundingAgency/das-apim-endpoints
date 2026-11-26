namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.LevyTransferMatching;

public class GetApplicationsToAutoExpireResponse
{
    public IEnumerable<int> ApplicationIdsToExpire { get; set; }
}
