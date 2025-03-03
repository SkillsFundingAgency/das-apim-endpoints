using SFA.DAS.SharedOuterApi.Interfaces;

public class EditApplicationApiRequest : IPutApiRequest
{
    public Guid ApplicationId { get; set; }

    public string PutUrl => $"/api/applications/{ApplicationId}";

    public object Data { get; set; }

}
