using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;

public class GetAllPagesApiRequest : IGetApiRequest
{
    private readonly Guid _sectionId;

    public GetAllPagesApiRequest(Guid sectionId)
    {
        _sectionId = sectionId;
    }

    public string GetUrl => $"/api/pages/section/{_sectionId}";
}