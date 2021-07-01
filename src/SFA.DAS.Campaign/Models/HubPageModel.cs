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
        public PageContent MainContent { get; set; }

        public HubPageModel Build(CmsContent hub)
        {
            if (hub.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var item = hub.Items.FirstOrDefault();

            Enum.TryParse<PageType>(item.Sys.ContentType.Sys.Id, true, out var pageTypeResult);

            var contentItems = new List<ContentItem>();

            if (item.Fields.Content?.Content == null)
            {
                return GenerateHubPageModel(item, pageTypeResult, contentItems);
            }

            foreach (var contentItem in item.Fields.Content.Content)
            {
                ProcessContentNodeTypes(hub, contentItem, contentItems);
                ProcessListNodeTypes(contentItem, contentItems);
                ProcessEmbeddedAssetBlockNodeTypes(hub, contentItem, contentItems);
            }

            return GenerateHubPageModel(item, pageTypeResult, contentItems);
        }

        private static void ProcessEmbeddedAssetBlockNodeTypes(CmsContent hub, SubContentItems contentItem, List<ContentItem> contentItems)
        {
            if (contentItem.NodeType.NodeTypeIsEmbeddedAssetBlock())
            {
                contentItems.Add(new ContentItem
                {
                    Type = contentItem.NodeType,
                    EmbeddedResource = hub.GetEmbeddedResource(contentItem.Data.Target.Sys.Id)
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
                    TableValue = contentItem.GetListItems(),
                });
            }
        }

        private static void ProcessContentNodeTypes(CmsContent hub, SubContentItems contentItem, List<ContentItem> contentItems)
        {
            if (contentItem.NodeType.NodeTypeIsContent())
            {
                contentItems.Add(new ContentItem
                {
                    Type = contentItem.NodeType,
                    Values = contentItem.BuildParagraph(),
                    TableValue = contentItem.BuildTable(hub)
                });
            }
        }

        private static HubPageModel GenerateHubPageModel(Item item, PageType pageTypeResult, List<ContentItem> contentItems)
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
                MainContent = new PageContent
                {
                    Items = contentItems
                }
            };
        }

        public class HubContent
        {
            public List<ContentItem> Items { get; set; }
        }
    }
}
