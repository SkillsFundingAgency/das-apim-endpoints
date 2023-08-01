using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;

public class GetFrameworkQueryRequest : IGetApiRequest
{
    public string TrainingCode { get; }

    public GetFrameworkQueryRequest(string trainingCode)
    {
        TrainingCode = trainingCode;
    }

    public string GetUrl => $"api/courses/Frameworks/{TrainingCode}";
}