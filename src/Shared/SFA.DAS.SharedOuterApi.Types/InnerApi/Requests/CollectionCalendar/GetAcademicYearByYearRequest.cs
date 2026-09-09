using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.CollectionCalendar;

public class GetAcademicYearByYearRequest(int year) : IGetApiRequest
{
    public string GetUrl => $"academicyears/{year}";
}