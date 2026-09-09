using SFA.DAS.Apim.Shared.Interfaces;

public class SaveSurveyApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/Survey";

    public object Data { get; set; }

}
