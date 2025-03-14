using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public class GetAcademicYearsLatestRequest : IGetApiRequest
{
    public GetAcademicYearsLatestRequest()
    {
    }

    public string GetUrl => $"api/AcademicYears/latest";
}