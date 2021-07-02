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

        private static List<PageModel> ProcessCards(CmsContent hub)
        {
            return hub.Includes?.Entry != null
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
                    .Select(entry => new PageModel
                    {
                        Slug = entry.Fields.Slug,
                        Summary = entry.Fields.Summary,
                        Title = entry.Fields.Title,
                        HubType = entry.Fields.HubType,
                        MetaDescription = entry.Fields.MetaDescription
                    })
                    .ToList()
                : new List<PageModel>();
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

        private static HubPageModel GenerateHubPageModel(Item item, PageType pageTypeResult, List<PageModel> cards, ContentItem headerImage)
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
            public List<PageModel> Cards { get; set; }
            
        }
    }
}
