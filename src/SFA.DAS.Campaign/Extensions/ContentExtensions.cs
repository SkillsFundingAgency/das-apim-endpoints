using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.Extensions
{
    public static class ContentExtensions
    {
        public const string ParagraphNodeTypeKey = "paragraph";
        public const string HyperLinkNodeTypeKey = "hyperlink";
        public const string TextNodeTypeKey = "text";
        public const string EmbeddedEntryInlineNodeTypeKey = "embedded-entry-inline";
        public const string BlockQuoteNodeTypeKey = "blockquote";
        public const string HorizontalRuleNodeTypeKey = "hr";
        public const string HeadingNodeTypeKey = "heading";
        public const string UnorderedListNodeTypeKey = "unordered-list";
        public const string OrderedListNodeTypeKey = "ordered-list";
        public const string EmbeddedAssetBlockNodeTypeKey = "embedded-asset-block";

        public static bool ContentItemsAreNullOrEmpty(this CmsContent pageContent)
        {
            if (pageContent.Total == 0)
            {
                return true;
            }
            var item = pageContent.Items.FirstOrDefault();

            return item == null;
        }

        public static List<string> BuildParagraph(this SubContentItems contentItemContent)
        {
            var returnList = new List<string>();

            foreach (var contentDefinition in contentItemContent.Content)
            {
                ProcessTextNodeType(contentDefinition, returnList);
                ProcessParagraphNodeType(contentDefinition, returnList);
                ProcessHyperLinkNodeType(contentDefinition, returnList);
            }

            return returnList;
        }

        public static List<List<string>> BuildTable(this SubContentItems contentItem, CmsContent article)
        {
            var data = new List<List<string>>();
            foreach (var subContentItem in contentItem.Content)
            {
                if (subContentItem.NodeType.Equals(EmbeddedEntryInlineNodeTypeKey))
                {
                    var linkedItemId = subContentItem.Data.Target.Sys.Id;
                    var item = article.Includes.Entry.FirstOrDefault(c => c.Sys.Id.Equals(linkedItemId, StringComparison.CurrentCultureIgnoreCase));
                    if (item != null)
                    {
                        data.AddRange(item.Fields.Table.TableData);
                    }
                }
            }

            return data;
        }

        public static bool NodeTypeIsContent(this string nodeType)
        {
            return nodeType.Equals(ParagraphNodeTypeKey,
                        StringComparison.CurrentCultureIgnoreCase) ||
                    nodeType.Equals(BlockQuoteNodeTypeKey,
                        StringComparison.CurrentCultureIgnoreCase) ||
                    nodeType.Equals(HorizontalRuleNodeTypeKey,
                        StringComparison.CurrentCultureIgnoreCase) ||
                    nodeType.StartsWith(HeadingNodeTypeKey,
                        StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool NodeTypeIsList(this string nodeType)
        {
            return nodeType.Equals(UnorderedListNodeTypeKey, StringComparison.CurrentCultureIgnoreCase) ||
                   nodeType.Equals(OrderedListNodeTypeKey, StringComparison.CurrentCultureIgnoreCase);
        }

        public static bool NodeTypeIsEmbeddedAssetBlock(this string nodeType)
        {
            return nodeType.Equals(EmbeddedAssetBlockNodeTypeKey, StringComparison.CurrentCultureIgnoreCase) ;
        }

        public static CmsPageModel.ResourceItem GetEmbeddedResource(this CmsContent article, string id)
        {
            var embeddedResource = article.Includes.Asset.FirstOrDefault(c => c.Sys.Id.Equals(id));

            if (embeddedResource != null)
            {
                return new CmsPageModel.ResourceItem
                {
                    Id = id,
                    Title = embeddedResource.Fields.Title,
                    Description = embeddedResource.Fields.Description,
                    FileName = embeddedResource.Fields.File.FileName,
                    Url = $"https:{embeddedResource.Fields.File.Url}",
                    ContentType = embeddedResource.Fields.File.ContentType,
                    Size = embeddedResource.Fields.File.Details.Size
                };
            }

            return new CmsPageModel.ResourceItem();
        }

        public static List<List<string>> GetListItems(this SubContentItems contentItems)
        {
            var returnList = new List<List<string>>();
            foreach (var relatedContent in contentItems.Content)
            {
                if (relatedContent.NodeType != "list-item")
                {
                    continue;
                }
                foreach (var content in relatedContent.Content)
                {
                    var list = new List<string>();
                    switch (content.NodeType)
                    {
                        case "paragraph":
                            {
                                foreach (var innerContent in content.Content)
                                {
                                    if (innerContent.NodeType.Equals("text"))
                                    {
                                        var fontEffect = innerContent.Marks?.FirstOrDefault()?.Type;
                                        list.Add($"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{innerContent.Value}");
                                    }
                                    if (innerContent.NodeType.Equals("hyperlink"))
                                    {
                                        list.Add($"[{innerContent.Content.FirstOrDefault().Value}]({innerContent.Data.Uri})");
                                    }
                                }
                                break;
                            }
                        case "text":
                            var font = content.Marks?.FirstOrDefault()?.Type;
                            list.Add($"{(string.IsNullOrWhiteSpace(font) ? "" : $"[{font}]")}{content.Value}");
                            break;
                        case "hyperlink":
                            list.Add($"[{content.Content.FirstOrDefault().Value}]({content.Data.Uri})");
                            break;
                    }
                    returnList.Add(list);
                }
            }

            return returnList;
        }

        private static void ProcessHyperLinkNodeType(ContentDefinition contentDefinition, List<string> returnList)
        {
            if (contentDefinition.NodeType.Equals(HyperLinkNodeTypeKey))
            {
                returnList.Add($"[{contentDefinition.Content.FirstOrDefault().Value}]({contentDefinition.Data.Uri})");
            }
        }

        private static void ProcessParagraphNodeType(ContentDefinition contentDefinition, List<string> returnList)
        {
            if (contentDefinition.NodeType.Equals(ParagraphNodeTypeKey))
            {
                foreach (var content in contentDefinition.Content)
                {
                    var fontEffect = content.Marks?.FirstOrDefault()?.Type;
                    returnList.Add($"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{content.Value}");
                }
            }
        }

        private static void ProcessTextNodeType(ContentDefinition contentDefinition, List<string> returnList)
        {
            if (contentDefinition.NodeType.Equals(TextNodeTypeKey))
            {
                var fontEffect = contentDefinition.Marks?.FirstOrDefault()?.Type;

                returnList.Add($"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{contentDefinition.Value}");
            }
        }
    }
}
