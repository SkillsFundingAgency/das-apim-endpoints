using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class PutCourseTypesRequest : IPutApiRequest
{
    public int Ukprn { get; set; }
    public string PutUrl => $"/providers/{Ukprn}/course-types";
    public object? Data { get; set; }

    public PutCourseTypesRequest(int ukprn, AddCourseTypesModel data)
    {
        Ukprn = ukprn;
        Data = data;
    }
}
