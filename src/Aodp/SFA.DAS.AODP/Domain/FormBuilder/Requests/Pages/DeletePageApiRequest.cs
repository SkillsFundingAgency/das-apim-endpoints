using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Pages;

public class DeletePageApiRequest : IDeleteApiRequest
{
    private readonly Guid _pageId;

    public DeletePageApiRequest(Guid pageId)
    {
        _pageId = pageId;
    }

    public string DeleteUrl => $"/api/pages/{_pageId}";
}