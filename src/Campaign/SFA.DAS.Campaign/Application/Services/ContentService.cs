using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.Campaign.Application.Services
{
    public class ContentService : IContentService
    {
        public bool HasContent(ApiResponse<CmsContent> cmsContent)
        {
            return !cmsContent.Body.ContentItemsAreNullOrEmpty();
        }
    }
}