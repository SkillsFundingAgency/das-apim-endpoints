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

        public List<PageModel> RelatedArticles { get; set; }
        public List<ResourceItem> Attachments { get; set; }

        public CmsPageModel Build(CmsContent article)
        {
            if (article.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var item = article.Items.FirstOrDefault();

            Enum.TryParse<PageType>(item.Sys.ContentType.Sys.Id, true, out var pageTypeResult);

            var contentItems = new List<ContentItem>();

            if (item.Fields.Content?.Content == null)
            {
                return GenerateCmsPageModel(article, item, pageTypeResult, contentItems, null);
            }

            foreach (var contentItem in item.Fields.Content.Content)
            {
                ProcessContentNodeTypes(article, contentItem, contentItems);
                ProcessListNodeTypes(contentItem, contentItems);
                ProcessEmbeddedAssetBlockNodeTypes(article, contentItem, contentItems);
            }

            var parentPage = article.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(item.Fields.LandingPage.Sys.Id));

            return GenerateCmsPageModel(article, item, pageTypeResult, contentItems, parentPage);
        }

        private static void ProcessEmbeddedAssetBlockNodeTypes(CmsContent article, SubContentItems contentItem,
            List<ContentItem> contentItems)
        {
            if (contentItem.NodeType.NodeTypeIsEmbeddedAssetBlock())
            {
                contentItems.Add(new ContentItem
                {
                    Type = contentItem.NodeType,
                    EmbeddedResource = article.GetEmbeddedResource(contentItem.Data.Target.Sys.Id)
                });
            }
        }

        private static void ProcessListNodeTypes(SubContentItems contentItem, List<ContentItem> contentItems)
        {
            if (contentItem.NodeType.NodeTypeIsList())
            {
                contentItems.Add(new ContentItem
                {
                    Type = contentItem.NodeType,
                    TableValue = contentItem.GetListItems()
                });
            }
        }

        private static void ProcessContentNodeTypes(CmsContent article, SubContentItems contentItem, List<ContentItem> contentItems)
        {
            if (contentItem.NodeType.NodeTypeIsContent())
            {
                contentItems.Add(new ContentItem
                {
                    Type = contentItem.NodeType,
                    Values = contentItem.BuildParagraph(),
                    TableValue = contentItem.BuildTable(article)
                });
            }
        }

        private CmsPageModel GenerateCmsPageModel(CmsContent article, Item item, PageType pageTypeResult, List<ContentItem> contentItems,
            Entry parentPage)
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
                    : null
            };
        }

        public PageModel ParentPage { get; set; }

        public class PageContent
        {
            public List<ContentItem> Items { get; set; }
        }
    }
}