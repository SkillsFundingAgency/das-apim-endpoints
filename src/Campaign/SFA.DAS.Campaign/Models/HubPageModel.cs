using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class HubPageModel
    {
        public PageModel PageAttributes { get; set; }
        public HubContent MainContent { get; set; }
        public MenuPageModel.MenuPageContent MenuContent { get; set; }
        public BannerPageModel BannerModels { get; set; }

        public HubPageModel Build(HubCmsContent hub, MenuPageModel.MenuPageContent menu, BannerPageModel banners)
        {
            if (hub.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var item = hub.Items.FirstOrDefault();

            Enum.TryParse<PageType>(item.Sys.ContentType.Sys.Id, true, out var pageTypeResult);

            return GenerateHubPageModel(item, pageTypeResult,
                ProcessCards(hub, item.Fields.Cards),
                ProcessCards(hub, item.Fields.Cards2),
                ProcessCards(hub, item.Fields.Cards3),
                ProcessSections(hub, item.Fields.Sections),
                ProcessHeaderImage(hub, item), menu, banners);
        }

        private static readonly PageType[] CardPageTypes = { PageType.Article, PageType.LandingPage };

        private static List<CardPageModel> ProcessCards(HubCmsContent hub, List<HubLink> cardItems)
        {
            if (cardItems == null)
            {
                return new List<CardPageModel>();
            }

            var cards = hub.Includes?.Entry != null
                ? hub
                    .Includes
                    .Entry.Where(c => c.Sys?.ContentType?.Sys?.Type != null
                                      && c.Sys.ContentType.Sys.Type.Equals("link",
                                          StringComparison.CurrentCultureIgnoreCase)
                                      && c.Sys.ContentType.Sys.LinkType.Equals("ContentType",
                                          StringComparison.CurrentCultureIgnoreCase)
                                      && Enum.TryParse<PageType>(c.Sys.ContentType.Sys.Id, true, out var type) &&
                                      CardPageTypes.Contains(type) &&
                                      cardItems.FirstOrDefault(o => o.Sys.Id == c.Sys.Id) != null
                    )
                    .Select(entry => new CardPageModel
                    {
                        Id = entry.Sys.Id,
                        Slug = entry.Fields.Slug,
                        Summary = entry.Fields.Summary,
                        Title = entry.Fields.Title,
                        HubType = entry.Fields.HubType,
                        MetaDescription = entry.Fields.MetaDescription,
                        PageType = GetPageType(entry),
                        LandingPage = SetLandingPageDetails(hub, entry)
                    })
                    .ToList()
                : new List<CardPageModel>();

            if (!cards.Any())
            {
                return cards;
            }

            for (var i = 0; i < cardItems.Count; i++)
            {
                cards = cards.OrderBy(o => o.Id == cardItems[i].Sys.Id).ToList();
            }

            return cards;
        }

        private static PageType GetPageType(HubEntry entry)
        {
            Enum.TryParse<PageType>(entry.Sys.ContentType.Sys.Id, true, out var pageType);

            return pageType;
        }

        private static UrlDetails SetLandingPageDetails(HubCmsContent hub, HubEntry entry)
        {
            if (entry.Fields.LandingPage?.Sys?.Id == null)
            {
                return new UrlDetails();
            }

            var parentPage = hub.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(entry.Fields.LandingPage.Sys.Id));

            return new UrlDetails
            {
                Hub = parentPage?.Fields.HubType,
                Title = parentPage?.Fields.Title,
                Slug = parentPage?.Fields.Slug
            };
        }

        private static List<HubSectionModel> ProcessSections(HubCmsContent hub, List<HubLink> sectionItems)
        {
            if (sectionItems == null || hub.Includes?.Entry == null)
            {
                return new List<HubSectionModel>();
            }

            return sectionItems
                .Select(section => FindEntry(hub, section?.Sys?.Id))
                .Where(entry => entry != null
                                && ContentfulConstants.HubSectionContentTypeId.Equals(entry.Sys.ContentType?.Sys?.Id,
                                    StringComparison.CurrentCultureIgnoreCase))
                .Select(entry => new HubSectionModel
                {
                    SectionType = entry.Fields.SectionType,
                    Heading = entry.Fields.Heading,
                    Introduction = entry.Fields.Introduction,
                    Image = ProcessSectionImage(hub, entry),
                    StepperLinks = ProcessSectionLinks(hub, entry.Fields.StepperLinks),
                    StandardLinks = ProcessSectionLinks(hub, entry.Fields.StandardLinks),
                    CtaPanel = ProcessCtaPanel(hub, entry.Fields.CtaPanel)
                })
                .ToList();
        }

        private static List<HubSectionLinkModel> ProcessSectionLinks(HubCmsContent hub, List<HubLink> linkItems)
        {
            if (linkItems == null || hub.Includes?.Entry == null)
            {
                return new List<HubSectionLinkModel>();
            }

            return linkItems
                .Select(link => FindEntry(hub, link?.Sys?.Id))
                .Where(entry => entry?.Sys?.ContentType?.Sys?.Id != null)
                .Select(entry => new HubSectionLinkModel
                {
                    Id = entry.Sys.Id,
                    PageType = GetPageType(entry),
                    Title = entry.Fields.Title,
                    Slug = entry.Fields.Slug,
                    Summary = entry.Fields.Summary,
                    MetaDescription = entry.Fields.MetaDescription,
                    LandingPage = SetLandingPageDetails(hub, entry),
                    CtaPanel = GetPageType(entry) == PageType.CtaPanel ? BuildCtaPanel(entry) : null
                })
                .ToList();
        }

        private static CtaPanelModel ProcessCtaPanel(HubCmsContent hub, HubLink ctaPanel)
        {
            var entry = FindEntry(hub, ctaPanel?.Sys?.Id);

            return entry == null ? null : BuildCtaPanel(entry);
        }

        private static CtaPanelModel BuildCtaPanel(HubEntry entry)
        {
            return new CtaPanelModel
            {
                Heading = entry.Fields.Heading,
                Description = entry.Fields.Description,
                Icon = entry.Fields.Icon,
                ButtonText = entry.Fields.ButtonText,
                Url = entry.Fields.Url
            };
        }

        private static ContentItem ProcessSectionImage(HubCmsContent hub, HubEntry entry)
        {
            if (entry.Fields.Image?.Sys?.Id == null || hub.Includes?.Asset == null)
            {
                return null;
            }

            return new ContentItem
            {
                Type = entry.Fields.Image.Sys.LinkType,
                EmbeddedResource = hub.GetEmbeddedResource(entry.Fields.Image.Sys.Id)
            };
        }

        private static HubEntry FindEntry(HubCmsContent hub, string id)
        {
            return id == null
                ? null
                : hub.Includes?.Entry?.FirstOrDefault(entry => id.Equals(entry.Sys?.Id));
        }

        private static ContentItem ProcessHeaderImage(HubCmsContent hub, HubItem item)
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

        private static HubPageModel GenerateHubPageModel(HubItem item, PageType pageTypeResult, List<CardPageModel> cards, List<CardPageModel> cards2, List<CardPageModel> cards3, List<HubSectionModel> sections, ContentItem headerImage, MenuPageModel.MenuPageContent menu, BannerPageModel banners)
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
                    Cards2 = cards2,
                    Cards3 = cards3,
                    CardsTitle = item.Fields.CardsTitle,
                    CardsTitle2 = item.Fields.CardsTitle2,
                    CardsTitle3 = item.Fields.CardsTitle3,
                    Sections = sections,
                    HeaderImage = headerImage
                },
                MenuContent = menu,
                BannerModels = banners
            };
        }

        public class HubContent
        {
            public ContentItem HeaderImage { get; set; }
            public string CardsTitle { get; set; }
            public List<CardPageModel> Cards { get; set; }
            public string CardsTitle2 { get; set; }
            public List<CardPageModel> Cards2 { get; set; }
            public string CardsTitle3 { get; set; }
            public List<CardPageModel> Cards3 { get; set; }
            public List<HubSectionModel> Sections { get; set; }
        }
    }
}
