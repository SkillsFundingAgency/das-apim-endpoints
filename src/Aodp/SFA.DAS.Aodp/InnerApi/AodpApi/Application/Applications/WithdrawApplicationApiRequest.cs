using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;

public class WithdrawApplicationApiRequest : IPostApiRequest
{
    public Guid ApplicationId { get; set; }

    public string PostUrl => $"/api/applications/{ApplicationId}/withdraw";

    public object Data { get; set; }

}