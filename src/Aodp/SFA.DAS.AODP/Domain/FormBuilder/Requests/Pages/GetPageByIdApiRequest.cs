using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;

public class GetPageByIdApiRequest : IGetApiRequest
{
    private readonly Guid _pageId;
    private readonly Guid _sectionId;

    public GetPageByIdApiRequest(Guid pageId, Guid sectionId)
    {
        _pageId = pageId;
        _sectionId = sectionId;
    }

    public string GetUrl => $"/api/pages/{_pageId}/section/{_sectionId}";
}