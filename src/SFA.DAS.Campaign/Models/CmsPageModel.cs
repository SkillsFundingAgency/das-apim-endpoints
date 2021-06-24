using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class PageModel
    {
        public PageType PageType { get; set; }
        public string Title { get; set; }
        public string MetaDescription { get; set; }
        public string Slug { get; set; }
        public string HubType { get; set; }
        public string Summary { get; set; }
    }

    public class CmsPageModel
    {
        public PageModel PageAttributes { get; set; }
        public PageContent MainContent { get; set; }

        public List<PageModel> RelatedArticles { get; set; }

        public CmsPageModel Build(CmsContent article)
        {
            var item = article.Items.FirstOrDefault();

            if (article.Total == 0 || item == null)
            {
                return null;
            }

            Enum.TryParse<PageType>(item.Sys.ContentType.Sys.Id, true, out var pageTypeResult);


            var contentItems = new List<ContentItem>();

            if (item.Fields.Content?.Content != null)
            {
                foreach (var contentItem in item.Fields.Content.Content)
                {
                    if (contentItem.NodeType.Equals("paragraph", StringComparison.CurrentCultureIgnoreCase) ||
                        contentItem.NodeType.StartsWith("heading", StringComparison.CurrentCultureIgnoreCase))
                    {
                        contentItems.Add(new ContentItem
                        {
                            Type = contentItem.NodeType,
                            Values = BuildParagraph(contentItem),
                            TableValue = BuildTable(contentItem, article)
                        });
                    }

                    if (contentItem.NodeType.Equals("unordered-list", StringComparison.CurrentCultureIgnoreCase))
                    {
                        contentItems.Add(new ContentItem
                        {
                            Type = contentItem.NodeType,
                            Values = GetListItems(contentItem),
                        });
                    }
                }
            }


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
                RelatedArticles = article.Includes?.Entry != null ? article
                    .Includes
                    .Entry.Where(c => c.Sys?.ContentType?.Sys?.Type != null
                                      && c.Sys.ContentType.Sys.Type.Equals("link", StringComparison.CurrentCultureIgnoreCase)
                                      && c.Sys.ContentType.Sys.LinkType.Equals("ContentType", StringComparison.CurrentCultureIgnoreCase)
                                      && Enum.TryParse<PageType>(c.Sys.ContentType.Sys.Id, true, out var type) && type == PageType.Article
                                      )
                    .Select(entry => new PageModel
                    {
                        Slug = entry.Fields.Slug,
                        Summary = entry.Fields.Summary,
                        Title = entry.Fields.Title,
                        HubType = entry.Fields.HubType,
                        MetaDescription = entry.Fields.MetaDescription
                    })
                    .ToList() : new List<PageModel>()
            };
        }

        private List<List<string>> BuildTable(SubContentItems contentItem, CmsContent article)
        {
            var data = new List<List<string>>();
            foreach (var subContentItem in contentItem.Content)
            {
                if (subContentItem.NodeType.Equals("embedded-entry-inline"))
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

        private List<string> BuildParagraph(SubContentItems contentItemContent)
        {
            var returnList = new List<string>();
            foreach (var contentDefinition in contentItemContent.Content)
            {
                if (contentDefinition.NodeType.Equals("text"))
                {
                    var fontEffect = contentDefinition.Marks?.FirstOrDefault()?.Type;

                    returnList.Add($"{(string.IsNullOrWhiteSpace(fontEffect) ? "" : $"[{fontEffect}]")}{contentDefinition.Value}");
                }
                if (contentDefinition.NodeType.Equals("hyperlink"))
                {
                    returnList.Add($"[{contentDefinition.Content.FirstOrDefault().Value}]({contentDefinition.Data.Uri})");
                }
            }

            return returnList;
        }

        private List<string> GetListItems(SubContentItems contentItems)
        {
            var returnList = new List<string>();
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
                        case "paragraph":
                        {
                            var sb = new StringBuilder();

                            foreach (var innerContent in content.Content)
                            {
                                if (innerContent.NodeType.Equals("text"))
                                {
                                    sb.Append(innerContent.Value);
                                }
                                if (innerContent.NodeType.Equals("hyperlink"))
                                {
                                    sb.Append($"[{innerContent.Content.FirstOrDefault().Value}]({innerContent.Data.Uri})");
                                }
                            }
                            returnList.Add(sb.ToString());

                            break;
                        }
                        case "text":
                            returnList.Add(content.Value);
                            break;
                        case "hyperlink":
                            returnList.Add($"[{content.Content.FirstOrDefault().Value}]({content.Data.Uri})");
                            break;
                    }
                }
            }

            return returnList;
        }
    }

    public class PageContent
    {
        public List<ContentItem> Items { get; set; }
    }


    public class ContentItem
    {
        public List<string> Values { get; set; }
        public string Type { get; set; }
        public List<List<string>> TableValue { get; set; }
    }


    public enum PageType
    {
        Unknown = 0,
        LandingPage = 1,
        Article = 2
    }
}