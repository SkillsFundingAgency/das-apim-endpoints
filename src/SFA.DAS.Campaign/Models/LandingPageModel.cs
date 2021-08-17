using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;
using static SFA.DAS.Campaign.Models.CmsPageModel;

namespace SFA.DAS.Campaign.Models
{
    public class LandingPageModel
    {
        public PageModel PageAttributes { get; set; }
        public HubContent MainContent { get; set; }
        public MenuPageModel.MenuPageContent MenuContent { get; set; }
        public BannerPageModel BannerModels { get; set; }
        public LandingPageModel Build(CmsContent hub, MenuPageModel.MenuPageContent menu, BannerPageModel banners)
        {
            if (hub.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var item = hub.Items.FirstOrDefault();

            Enum.TryParse<PageType>(item.Sys.ContentType.Sys.Id, true, out var pageTypeResult);

            return GenerateHubPageModel(item, pageTypeResult, ProcessCards(hub), ProcessHeaderImage(hub, item), menu, banners);
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
                                      type == PageType.Article &&
                                      hub.Items[0].Fields.Cards.FirstOrDefault(o => o.Sys.Id == c.Sys.Id) != null
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

        private static UrlDetails SetLandingPageDetails(CmsContent hub, Entry entry)
        {
            var parentPage = hub.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(entry.Fields.LandingPage.Sys.Id));
            Item parentPageFromItem = null;
            
            if (parentPage == null)
            {
                parentPageFromItem = hub.Items.FirstOrDefault(c => c.Sys.Id.Equals(entry.Fields.LandingPage.Sys.Id));
            }

            if (parentPageFromItem == null && parentPage == null)
            {
                return new UrlDetails();
            }

            return new UrlDetails
            {
                Hub = parentPage != null ? parentPage.Fields.HubType : parentPageFromItem.Fields.HubType,
                Title = parentPage != null ? parentPage.Fields.Title : parentPageFromItem.Fields.Title,
                Slug = parentPage != null ? parentPage.Fields.Slug : parentPageFromItem.Fields.Slug
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

        private static LandingPageModel GenerateHubPageModel(Item item, PageType pageTypeResult, List<CardPageModel> cards, ContentItem headerImage, MenuPageModel.MenuPageContent menu, BannerPageModel banners)
        {
            return new LandingPageModel()
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
                },
                MenuContent = menu,
                BannerModels = banners
            };
        }

        public class HubContent
        {
            public ContentItem HeaderImage { get; set; }
            public List<CardPageModel> Cards { get; set; }
            
        }
    }
}
