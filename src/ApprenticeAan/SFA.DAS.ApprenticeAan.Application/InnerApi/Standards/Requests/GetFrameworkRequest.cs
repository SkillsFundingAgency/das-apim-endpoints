using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;

public class GetFrameworkRequest : IGetApiRequest
{
    public string TrainingCode { get; }

    public GetFrameworkRequest(string trainingCode)
    {
        TrainingCode = trainingCode;
    }

    public string GetUrl => $"api/courses/Frameworks/{TrainingCode}";
}