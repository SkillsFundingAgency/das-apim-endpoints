using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Requests;

public class GetContactRequest : IGetApiRequest
{
    public string GetUrl => $"providers/{Ukprn}/contact";
    public int Ukprn { get; }

    public GetContactRequest(int ukprn)
    {
        Ukprn = ukprn;
    }
}