using SFA.DAS.AdminRoatp.InnerApi.Models;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.AdminRoatp.InnerApi.Requests;

public class AddCourseTypesRequest : IPostApiRequest
{
    public int Ukprn { get; set; }
    public string PostUrl => $"/providers/{Ukprn}/course-types";
    public object Data { get; set; }

    public AddCourseTypesRequest(int ukprn, AddCourseTypesModel data)
    {
        Ukprn = ukprn;
        Data = data;
    }
}
