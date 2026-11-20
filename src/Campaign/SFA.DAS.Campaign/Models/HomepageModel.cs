using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class HomepageModel
    {
        public string PageTitle { get; set; }
        public string BrowserTitle { get; set; }
        public string IntroText { get; set; }
        public List<FeatureCardModel> FeatureCards { get; set; }

        public HomepageModel Build(CmsContent homepage)
        {
            if (homepage.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var item = homepage.Items.First();

            return new HomepageModel
            {
                PageTitle = item.Fields.PageTitle,
                BrowserTitle = item.Fields.BrowserTitle,
                IntroText = item.Fields.IntroText,
                FeatureCards = BuildFeatureCards(homepage, item)
            };
        }

        private static List<FeatureCardModel> BuildFeatureCards(CmsContent homepage, Item item)
        {
            var cardLinks = item.Fields.FeatureCards;
            var entries = homepage.Includes?.Entry;

            if (cardLinks == null || entries == null)
            {
                return new List<FeatureCardModel>();
            }

            // Walk the link array rather than the includes so the editor's ordering is preserved.
            return cardLinks
                .Select(link => entries.FirstOrDefault(entry => entry.Sys?.Id != null && entry.Sys.Id.Equals(link.Sys?.Id)))
                .Where(entry => entry?.Fields != null)
                .Select(entry => new FeatureCardModel
                {
                    Id = entry.Sys.Id,
                    Heading = entry.Fields.Heading,
                    Description = entry.Fields.Description,
                    Audience = entry.Fields.Audience,
                    LinkType = entry.Fields.LinkType,
                    ExternalUrl = entry.Fields.ExternalUrl,
                    InternalPage = BuildInternalPage(entries, entry.Fields.InternalPage)
                })
                .ToList();
        }

        private static UrlDetails BuildInternalPage(List<Entry> entries, LandingPage internalPageLink)
        {
            var linkedId = internalPageLink?.Sys?.Id;
            if (linkedId == null)
            {
                return null;
            }

            var page = entries.FirstOrDefault(entry => entry.Sys?.Id != null && entry.Sys.Id.Equals(linkedId));
            if (page?.Fields == null)
            {
                return null;
            }

            var parent = page.Fields.LandingPage?.Sys?.Id != null
                ? entries.FirstOrDefault(entry => entry.Sys?.Id != null && entry.Sys.Id.Equals(page.Fields.LandingPage.Sys.Id))
                : null;

            return new UrlDetails
            {
                Title = page.Fields.Title,
                Slug = page.Fields.Slug,
                Hub = page.Fields.HubType,
                PageType = page.Sys.ContentType?.Sys?.Id?.GetPageTypeValue(),
                ParentSlug = parent?.Fields?.Slug
            };
        }

        public class FeatureCardModel
        {
            public string Id { get; set; }
            public string Heading { get; set; }
            public string Description { get; set; }
            public string Audience { get; set; }
            public string LinkType { get; set; }
            public string ExternalUrl { get; set; }
            public UrlDetails InternalPage { get; set; }
        }
    }
}
