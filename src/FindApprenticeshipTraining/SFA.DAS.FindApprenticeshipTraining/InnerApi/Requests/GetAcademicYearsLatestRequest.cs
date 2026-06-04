using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

public class GetAcademicYearsLatestRequest : IGetApiRequest
{
    public GetAcademicYearsLatestRequest()
    {
    }

    public string GetUrl => $"api/AcademicYears/latest";
}