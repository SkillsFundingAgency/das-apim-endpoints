using SFA.DAS.SharedOuterApi.Interfaces;

public class SaveSurveyApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/Survey";

    public object Data { get; set; }

}
