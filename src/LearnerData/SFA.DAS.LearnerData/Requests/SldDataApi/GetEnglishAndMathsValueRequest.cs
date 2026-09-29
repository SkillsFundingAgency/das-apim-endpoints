using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Requests.SldDataApi;

public class GetEnglishAndMathsValueRequest(string learnAimRef, DateOnly startDate) : IGetApiRequest
{
    public string GetUrl => $"api/lars/get-em-value/{Uri.EscapeDataString(learnAimRef)}/{startDate:yyyy-MM-dd}";
}
