using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.RoatpCourseManagement.InnerApi.Models;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class PostProviderEmailRequest : IPostApiRequest
{
    public int Ukprn { get; set; }
    public object Data { get; set; }
    public string PostUrl => $"api/email/{Ukprn}/send";

    public PostProviderEmailRequest(int ukprn, ProviderEmailModel data)
    {
        Ukprn = ukprn;
        Data = data;
    }
}
