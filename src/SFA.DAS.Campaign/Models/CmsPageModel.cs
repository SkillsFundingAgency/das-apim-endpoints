using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class CmsPageModel
    {
        public PageModel PageAttributes { get; set; }
        public PageContent MainContent { get; set; }
        public MenuPageModel.MenuPageContent MenuContent { get; set; }
        public BannerPageModel BannerModels { get; set; }
        public List<PageModel> RelatedArticles { get; set; }
        public List<ResourceItem> Attachments { get; set; }

        public CmsPageModel Build(CmsContent article, MenuPageModel.MenuPageContent menu, BannerPageModel banners)
        {
            if (article.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var item = article.Items.FirstOrDefault();

            var pageTypeResult = item.Sys.ContentType.Sys.Id.GetPageType();

            var contentItems = new List<ContentItem>();

            if (item.Fields.Content?.Content == null)
            {
                return GenerateCmsPageModel(article, item, pageTypeResult, contentItems, null, menu, banners);
            }

            foreach (var contentItem in item.Fields.Content.Content)
            {
                article.ProcessContentNodeTypes(contentItem, contentItems);
                contentItem.ProcessListNodeTypes(contentItems);
                article.ProcessEmbeddedAssetBlockNodeTypes(contentItem, contentItems);
            }

            var parentPage = article.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(item.Fields.LandingPage?.Sys?.Id));

            return GenerateCmsPageModel(article, item, pageTypeResult, contentItems, parentPage, menu, banners);
        }

       
        private CmsPageModel GenerateCmsPageModel(CmsContent article, Item item, PageType pageTypeResult, List<ContentItem> contentItems,
            Entry parentPage, MenuPageModel.MenuPageContent menu, BannerPageModel banners)
        {
            return new CmsPageModel
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
                MainContent = new PageContent
                {
                    Items = contentItems
                },
                Attachments = item.Fields.Attachments?.Select(attachment => article.GetEmbeddedResource(attachment.Sys.Id))
                    .ToList(),
                RelatedArticles = article.Includes?.Entry != null
                    ? article
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
                    : new List<PageModel>(),
                ParentPage = parentPage != null
                    ? new PageModel
                    {
                        Slug = parentPage.Fields.Slug,
                        Title = parentPage.Fields.Title,
                        Summary = parentPage.Fields.Summary,
                        HubType = parentPage.Fields.HubType,
                        MetaDescription = parentPage.Fields.MetaDescription
                    }
                    : null,
                MenuContent = menu,
                BannerModels = banners
            };
        }

        public PageModel ParentPage { get; set; }

        public class PageContent
        {
            public List<ContentItem> Items { get; set; }
        }
    }
}