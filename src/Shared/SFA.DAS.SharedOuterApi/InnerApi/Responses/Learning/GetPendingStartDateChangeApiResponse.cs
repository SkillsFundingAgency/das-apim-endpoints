namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning;

public class GetPendingStartDateChangeApiResponse
{
    public bool HasPendingStartDateChange { get; set; }
    public PendingStartDateChange PendingStartDateChange { get; set; }
}