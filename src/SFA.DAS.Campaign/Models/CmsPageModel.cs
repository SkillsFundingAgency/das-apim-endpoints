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
        public List<TabbedContentModel> TabbedContents { get; set; }
        public List<PageModel> RelatedArticles { get; set; }
        public List<ResourceItem> Attachments { get; set; }

        public CmsPageModel Build(CmsContent article, MenuPageModel.MenuPageContent menu)
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
                return GenerateCmsPageModel(article, item, pageTypeResult, contentItems, null, menu);
            }

            foreach (var contentItem in item.Fields.Content.Content)
            {
                ProcessContentNodeTypes(article, contentItem, contentItems);
                ProcessListNodeTypes(contentItem, contentItems);
                ProcessEmbeddedAssetBlockNodeTypes(article, contentItem, contentItems);
            }

            var parentPage =
                article.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(item.Fields.LandingPage?.Sys?.Id));

            return GenerateCmsPageModel(article, item, pageTypeResult, contentItems, parentPage, menu);
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

        private static void ProcessContentNodeTypes(CmsContent article, SubContentItems contentItem,
            List<ContentItem> contentItems)
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

        private CmsPageModel GenerateCmsPageModel(CmsContent article, Item item, PageType pageTypeResult,
            List<ContentItem> contentItems,
            Entry parentPage, MenuPageModel.MenuPageContent menu)
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
                Attachments = item.Fields.Attachments
                    ?.Select(attachment => article.GetEmbeddedResource(attachment.Sys.Id))
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
                TabbedContents = ProcessTabbedContent(article, item)
            };
        }

        public PageModel ParentPage { get; set; }

        public class PageContent
        {
            public List<ContentItem> Items { get; set; }

        }

        private static List<TabbedContentModel> ProcessTabbedContent(CmsContent article, Item item)
        {
            var tabbedContent = article.Includes.Entry.Where(c => c.Sys?.ContentType?.Sys?.Type != null
                                                        && c.Sys.ContentType.Sys.Type.Equals("link",
                                                            StringComparison.CurrentCultureIgnoreCase)
                                                        && c.Sys.ContentType.Sys.LinkType.Equals("ContentType",
                                                            StringComparison.CurrentCultureIgnoreCase)
                                                        && Enum.TryParse<PageType>(c.Sys.ContentType.Sys.Id, true,
                                                            out var type) &&
                                                        type == PageType.Tab &&
                                                        article.Items[0].Fields.TabbedContents
                                                            .FirstOrDefault(o => o.Sys.Id == c.Sys.Id) != null);
            if (!tabbedContent.Any())
            {
                return null;
            }

            var tabbedContentModels = new List<TabbedContentModel>();

            foreach (var tab in tabbedContent)
            {
                var contentItems = new List<ContentItem>();
                var tabModel = new TabbedContentModel
                {
                    TabName = tab.Fields.TabName,
                    TabTitle = tab.Fields.TabTitle,
                    FindTraineeship = tab.Fields.FindTraineeship
                };

                foreach (var contentItem in tab.Fields.TabContent.Content)
                {
                    ProcessContentNodeTypes(article, contentItem, contentItems);
                    ProcessListNodeTypes(contentItem, contentItems);
                    ProcessEmbeddedAssetBlockNodeTypes(article, contentItem, contentItems);
                }

                tabModel.Content.Items = contentItems;

                tabbedContentModels.Add(tabModel);
            }

            return tabbedContentModels;
        }


        private static void ProcessContentNodeTypes(CmsContent article, FluffyContent contentItem,
            List<ContentItem> contentItems)
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

        private static void ProcessListNodeTypes(FluffyContent contentItem, List<ContentItem> contentItems)
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

        private static void ProcessEmbeddedAssetBlockNodeTypes(CmsContent article, FluffyContent contentItem,
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
        //var cards = hub.Includes?.Entry != null
        //    ? hub
        //        .Includes
        //.Entry.Where(c => c.Sys?.ContentType?.Sys?.Type != null
        //                  && c.Sys.ContentType.Sys.Type.Equals("link",
        //                      StringComparison.CurrentCultureIgnoreCase)
        //                  && c.Sys.ContentType.Sys.LinkType.Equals("ContentType",
        //                      StringComparison.CurrentCultureIgnoreCase)
        //                  && Enum.TryParse<PageType>(c.Sys.ContentType.Sys.Id, true, out var type) &&
        //                  type == PageType.Article &&
        //                  hub.Items[0].Fields.Cards.FirstOrDefault(o => o.Sys.Id == c.Sys.Id) != null
        //)
        //        .Select(entry => new CardPageModel
        //        {
        //            Id = entry.Sys.Id,
        //            Slug = entry.Fields.Slug,
        //            Summary = entry.Fields.Summary,
        //            Title = entry.Fields.Title,
        //            HubType = entry.Fields.HubType,
        //            MetaDescription = entry.Fields.MetaDescription,
        //            LandingPage = SetLandingPageDetails(hub, entry)
        //        })
        //        .ToList()
        //    : new List<CardPageModel>();

        //if (!cards.Any())
        //{
        //    return cards;
        //}

        //for (var i = 0; i < hub.Items[0].Fields.Cards.Count; i++)
        //{
        //    cards = cards.OrderBy(o => o.Id == hub.Items[0].Fields.Cards[i].Sys.Id).ToList();
        //}

        //return cards;
    }

   
}
