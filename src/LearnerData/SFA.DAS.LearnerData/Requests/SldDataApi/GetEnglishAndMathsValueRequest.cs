using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.SldDataApi;

public class GetEnglishAndMathsValueRequest(string learnAimRef, DateOnly startDate) : IGetApiRequest
{
    public string GetUrl => $"api/v1.0/lars/get-em-value/{Uri.EscapeDataString(learnAimRef)}/{startDate:yyyy-MM-dd}";
}
