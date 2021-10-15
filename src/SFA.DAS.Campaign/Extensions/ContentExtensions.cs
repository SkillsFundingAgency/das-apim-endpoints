using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Application.Queries.Banner;
using SFA.DAS.Campaign.Application.Queries.Menu;
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
            if (pageContent == null || pageContent.Total == 0)
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

        public static List<string> BuildParagraph(this FluffyContent contentItemContent)
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

        public static List<List<string>> BuildTable(this FluffyContent contentItem, CmsContent article)
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
            return nodeType.Equals(EmbeddedAssetBlockNodeTypeKey, StringComparison.CurrentCultureIgnoreCase);
        }

        public static ResourceItem GetEmbeddedResource(this CmsContent article, string id)
        {
            var embeddedResource = article.Includes.Asset.FirstOrDefault(c => c.Sys.Id.Equals(id));

            if (embeddedResource != null)
            {
                return new ResourceItem
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

            return new ResourceItem();
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
                    switch (content.NodeType)
                    {
                        case ParagraphNodeTypeKey:
                        case TextNodeTypeKey:
                            // do something
                            goto case HyperLinkNodeTypeKey;
                        case HyperLinkNodeTypeKey:
                            // do something else
                            ProcessNodeType(returnList, content);
                            break;
                        default:
                            ProcessListItemRelatedContentForGet(content, returnList);
                            break;
                    }
                }
            }

            return returnList;
        }

        public static List<List<string>> GetListItems(this FluffyContent contentItems)
        {
            var returnList = new List<List<string>>();
            foreach (var relatedContent in contentItems.Content)
            {
                if (relatedContent.NodeType != "list-item")
                {
                    continue;
                }

                ProcessListItemRelatedContentForGet(relatedContent, returnList);
            }

            return returnList;
        }

        private static void ProcessListItemRelatedContentForGet(RelatedContent relatedContent, List<List<string>> returnList)
        {
            if (relatedContent?.Content == null)
            {
                return;
            }

            foreach (var content in relatedContent.Content)
            {
                switch (content.NodeType)
                {
                    case ParagraphNodeTypeKey: 
                    case TextNodeTypeKey:
                        // do something
                        goto case HyperLinkNodeTypeKey;
                    case HyperLinkNodeTypeKey:
                        // do something else
                        ProcessNodeType(returnList, content);
                        break;
                }
                
                //ProcessNodeType(returnList, content);
            }
        }

        private static void ProcessNodeType(List<List<string>> returnList, RelatedContent content)
        {
            var list = new List<string>();
            switch (content.NodeType)
            {
                case ParagraphNodeTypeKey:
                    {
                        foreach (var innerContent in content.Content)
                        {
                            if (innerContent.NodeType.Equals(TextNodeTypeKey))
                            {
                                var fontEffect = innerContent.Marks?.FirstOrDefault()?.Type;
                                list.Add(
                                    $"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{innerContent.Value}");
                            }

                            if (innerContent.NodeType.Equals(HyperLinkNodeTypeKey))
                            {
                                list.Add(
                                    $"[{innerContent.Content.FirstOrDefault().Value}]({innerContent.Data.Uri})");
                            }
                        }

                        break;
                    }
                case TextNodeTypeKey:
                    var font = content.Marks?.FirstOrDefault()?.Type;
                    list.Add($"{(string.IsNullOrWhiteSpace(font) ? "" : $"[{font}]")}{content.Value}");
                    break;
                case HyperLinkNodeTypeKey:
                    list.Add($"[{content.Content.FirstOrDefault().Value}]({content.Data.Uri})");
                    break;
            }

            returnList.Add(list);
        }

        public static string GetPageTypeValue(this string sysId)
        {
            Enum.TryParse<PageType>(sysId, true, out var pageTypeResult);

            return pageTypeResult.ToString();
        }

        public static PageType GetPageType(this string sysId)
        {
            Enum.TryParse<PageType>(sysId, true, out var pageTypeResult);

            return pageTypeResult;
        }

        public static async Task<MenuPageModel> RetrieveMenu(this IMediator mediator, CancellationToken cancellationToken = default)
        {
            var topLevelMenuResult = mediator.Send(new GetMenuQuery { MenuType = "TopLevel" }, cancellationToken);
            var apprenticesMenuResult = mediator.Send(new GetMenuQuery { MenuType = "Apprentices" }, cancellationToken);
            var employersMenuResult = mediator.Send(new GetMenuQuery { MenuType = "Employers" }, cancellationToken);
            var influencersMenuResult = mediator.Send(new GetMenuQuery { MenuType = "Influencers" }, cancellationToken);

            await Task.WhenAll(topLevelMenuResult, apprenticesMenuResult, employersMenuResult, influencersMenuResult);

            var menuModel = new MenuPageModel
            {
                MainContent = new MenuPageModel.MenuPageContent
                {
                    Apprentices = apprenticesMenuResult.Result.PageModel.MainContent,
                    Employers = employersMenuResult.Result.PageModel.MainContent,
                    Influencers = influencersMenuResult.Result.PageModel.MainContent,
                    TopLevel = topLevelMenuResult.Result.PageModel.MainContent
                }
            };

            return menuModel;
        }

        public static async Task<BannerPageModel> RetrieveBanners(this IMediator mediator, CancellationToken cancellationToken = default)
        {
            var banners = await  mediator.Send(new GetBannerQuery(), cancellationToken);
          
            var bannerModels = new BannerPageModel
            {
                MainContent = banners.PageModel == null ? new List<BannerPageModel.BannerPageContent>() : banners.PageModel.MainContent
            };

            return bannerModels;
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
            if (!contentDefinition.NodeType.Equals(ParagraphNodeTypeKey))
            {
                return;
            }

            foreach (var content in contentDefinition.Content)
            {
                var fontEffect = content.Marks?.FirstOrDefault()?.Type;
                returnList.Add($"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{content.Value}");
            }
        }

        private static void ProcessTextNodeType(ContentDefinition contentDefinition, List<string> returnList)
        {
            if (!contentDefinition.NodeType.Equals(TextNodeTypeKey))
            {
                return;
            }

            var fontEffect = contentDefinition.Marks?.FirstOrDefault()?.Type;

            returnList.Add($"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{contentDefinition.Value}");
        }

        public static void ProcessEmbeddedAssetBlockNodeTypes(this CmsContent article, SubContentItems contentItem,
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

        public static void ProcessListNodeTypes(this SubContentItems contentItem, List<ContentItem> contentItems)
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

        public static void ProcessContentNodeTypes(this CmsContent article, SubContentItems contentItem, List<ContentItem> contentItems)
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

        private static void ProcessTextNodeType(RelatedContent contentDefinition, List<string> returnList)
        {
            if (!contentDefinition.NodeType.Equals(TextNodeTypeKey))
            {
                return;
            }

            var fontEffect = contentDefinition.Marks?.FirstOrDefault()?.Type;

            returnList.Add($"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{contentDefinition.Value}");
        }

        private static void ProcessParagraphNodeType(RelatedContent contentDefinition, List<string> returnList)
        {
            if (!contentDefinition.NodeType.Equals(ParagraphNodeTypeKey))
            {
                return;
            }

            foreach (var content in contentDefinition.Content)
            {
                var fontEffect = content.Marks?.FirstOrDefault()?.Type;
                returnList.Add($"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{content.Value}");
            }
        }

        private static void ProcessHyperLinkNodeType(RelatedContent contentDefinition, List<string> returnList)
        {
            if (contentDefinition.NodeType.Equals(HyperLinkNodeTypeKey))
            {
                returnList.Add($"[{contentDefinition.Content.FirstOrDefault().Value}]({contentDefinition.Data.Uri})");
            }
        }


    }
}
