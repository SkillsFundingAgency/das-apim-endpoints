namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Learning;

public class GetPendingStartDateChangeApiResponse
{
    public bool HasPendingStartDateChange { get; set; }
    public PendingStartDateChange PendingStartDateChange { get; set; }
}