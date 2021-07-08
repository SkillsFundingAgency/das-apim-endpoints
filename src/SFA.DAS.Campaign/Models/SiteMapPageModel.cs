using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class SiteMapPageModel
    {
        public SiteMapContent MainContent { get; set; }

        public SiteMapPageModel Build(CmsContent hub)
        {
            if (hub.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            return GenerateHubPageModel(SetLandingPageDetails(hub));
        }

        private static List<UrlDetails> SetLandingPageDetails(CmsContent hub)
        {
            return hub.Items.Select(item => new UrlDetails
            {
                Hub = item?.Fields.HubType, Title = item?.Fields.Title, Slug = item?.Fields.Slug,
                PageType = GetPageType(item.Sys.ContentType.Sys.Id).ToString()
            }).ToList();
        }

        private static string GetPageType(string sysId)
        {
            Enum.TryParse<PageType>(sysId, true, out var pageTypeResult);

            return pageTypeResult.ToString();
        }

        private static SiteMapPageModel GenerateHubPageModel(List<UrlDetails> pages)
        {
            return new SiteMapPageModel()
            {
                MainContent = new SiteMapContent
                {
                    Pages = pages
                }
            };
        }

        public class SiteMapContent
        {
            public SiteMapContent()
            {
                Pages = new List<UrlDetails>();
            }
            public List<UrlDetails> Pages { get; set; }
        }
    }
}
