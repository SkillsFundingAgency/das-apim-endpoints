using SFA.DAS.SharedOuterApi.Interfaces;

public class CreateApplicationApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/applications";

    public object Data { get; set; }

}
