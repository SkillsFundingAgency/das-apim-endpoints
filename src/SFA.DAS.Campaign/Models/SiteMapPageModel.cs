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

            var item = hub.Items.FirstOrDefault();

            Enum.TryParse<PageType>(item.Sys.ContentType.Sys.Id, true, out var pageTypeResult);

            return GenerateHubPageModel(new List<UrlDetails>());
        }

       
        private static UrlDetails SetLandingPageDetails(CmsContent hub, Entry entry)
        {
            var parentPage = hub.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(entry.Fields.LandingPage?.Sys.Id));
            
            return new UrlDetails
            {
                Hub = parentPage?.Fields.HubType,
                Title = parentPage?.Fields.Title,
                Slug = parentPage?.Fields.Slug
            };
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
            public List<UrlDetails> Pages { get; set; }
        }
    }
}
