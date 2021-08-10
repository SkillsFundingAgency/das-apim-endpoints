using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Campaign.Interfaces
{
    public interface IContentService
    {
        bool HasContent(ApiResponse<CmsContent> cmsContent);
    }
}