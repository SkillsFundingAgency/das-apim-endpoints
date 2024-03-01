using System;
using System.Collections.Generic;
using System.Linq;
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
                PageType = item.Sys.ContentType.Sys.Id.GetPageTypeValue(), ParentSlug = ParentPageSlug(item, hub)
            }).ToList();
        }

        private static string ParentPageSlug(Item item, CmsContent hub)
        {
            if (string.Compare(item.Sys.ContentType.Sys.Id,"landingPage", StringComparison.OrdinalIgnoreCase) == 0)
            {
                return string.Empty;
            }

            var parentPage = hub.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(item.Fields.LandingPage?.Sys?.Id));
            return parentPage?.Fields?.Slug;
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
