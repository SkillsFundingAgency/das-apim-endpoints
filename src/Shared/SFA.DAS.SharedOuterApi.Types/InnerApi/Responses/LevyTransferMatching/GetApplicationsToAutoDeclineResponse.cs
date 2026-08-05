namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.LevyTransferMatching;

public class GetApplicationsToAutoDeclineResponse
{
    public IEnumerable<int> ApplicationIdsToDecline { get; set; }
}
