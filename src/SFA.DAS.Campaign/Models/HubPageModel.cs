using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;
using static SFA.DAS.Campaign.Models.CmsPageModel;

namespace SFA.DAS.Campaign.Models
{
    public class HubPageModel
    {
        public PageModel PageAttributes { get; set; }
        public HubContent MainContent { get; set; }

        public HubPageModel Build(CmsContent hub)
        {
            if (hub.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var item = hub.Items.FirstOrDefault();

            Enum.TryParse<PageType>(item.Sys.ContentType.Sys.Id, true, out var pageTypeResult);

            return GenerateHubPageModel(item, pageTypeResult, ProcessCards(hub), ProcessHeaderImage(hub, item));
        }

        private static List<CardPageModel> ProcessCards(CmsContent hub)
        {
            var cards = hub.Includes?.Entry != null
                ? hub
                    .Includes
                    .Entry.Where(c => c.Sys?.ContentType?.Sys?.Type != null
                                      && c.Sys.ContentType.Sys.Type.Equals("link",
                                          StringComparison.CurrentCultureIgnoreCase)
                                      && c.Sys.ContentType.Sys.LinkType.Equals("ContentType",
                                          StringComparison.CurrentCultureIgnoreCase)
                                      && Enum.TryParse<PageType>(c.Sys.ContentType.Sys.Id, true, out var type) &&
                                      type == PageType.Article
                    )
                    .Select(entry => new CardPageModel
                    {
                        Id = entry.Sys.Id,
                        Slug = entry.Fields.Slug,
                        Summary = entry.Fields.Summary,
                        Title = entry.Fields.Title,
                        HubType = entry.Fields.HubType,
                        MetaDescription = entry.Fields.MetaDescription,
                        LandingPage = SetLandingPageDetails(hub, entry)
                    })
                    .ToList()
                : new List<CardPageModel>();

            if (!cards.Any())
            {
                return cards;
            }

            for (var i = 0; i < hub.Items[0].Fields.Cards.Count; i++)
            {
                cards = cards.OrderBy(o => o.Id == hub.Items[0].Fields.Cards[i].Sys.Id).ToList();
            }

            return cards;
        }

        private static LandingPageModel SetLandingPageDetails(CmsContent hub, Entry entry)
        {
            var parentPage = hub.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(entry.Fields.LandingPage.Sys.Id));

            return new LandingPageModel
            {
                Hub = parentPage?.Fields.HubType.ToString(),
                Title = parentPage?.Fields.Title,
                Slug = parentPage?.Fields.Slug
            };
        }

        private static ContentItem ProcessHeaderImage(CmsContent hub, Item item)
        {
            if (item.Fields.HeaderImage == null)
            {
                return null;
            }

            return new ContentItem
            {
                Type = item.Fields.HeaderImage.Sys.LinkType,
                EmbeddedResource = hub.GetEmbeddedResource(item.Fields.HeaderImage.Sys.Id)
            };
        }

        private static HubPageModel GenerateHubPageModel(Item item, PageType pageTypeResult, List<CardPageModel> cards, ContentItem headerImage)
        {
            return new HubPageModel()
            {
                PageAttributes = new PageModel
                {
                    Title = item.Fields.Title,
                    Summary = item.Fields.Summary,
                    Slug = item.Fields.Slug,
                    HubType = item.Fields.HubType,
                    MetaDescription = item.Fields.MetaDescription,
                    PageType = pageTypeResult,
                },
                MainContent = new HubContent()
                {
                    Cards = cards,
                    HeaderImage = headerImage
                }
            };
        }

        public class HubContent
        {
            public ContentItem HeaderImage { get; set; }
            public List<CardPageModel> Cards { get; set; }
            
        }
    }
}
