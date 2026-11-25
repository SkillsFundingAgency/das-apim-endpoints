using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.CollectionCalendar;

public class GetAcademicYearByDateRequest(DateTime dateTime) : IGetApiRequest
{
    public readonly string _dateTime = dateTime.ToString("yyyy-MM-dd");
    public string GetUrl => $"academicyears?date={_dateTime}";
}