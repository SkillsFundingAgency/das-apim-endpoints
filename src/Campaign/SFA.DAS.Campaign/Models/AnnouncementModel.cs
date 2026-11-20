using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class AnnouncementModel
    {
        public AnnouncementContent MainContent { get; set; }
        public List<TabbedContentModel> Sections { get; set; }

        public AnnouncementModel Build(CmsContent announcement)
        {
            if (announcement.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var item = announcement.Items.First();

            var model = new AnnouncementModel
            {
                MainContent = new AnnouncementContent
                {
                    Id = item.Sys?.Id,
                    Headline = item.Fields.Headline,
                    Slug = item.Fields.Slug,
                    Intro = item.Fields.Intro,
                    PublishedDate = item.Fields.Date,
                    UpdatedDate = item.Fields.UpdatedDate,
                    Audience = item.Fields.Audience,
                    Category = announcement.GetAnnouncementCategoryTitle(item.Fields.Category)
                },
                Sections = announcement.BuildTabbedContent(item.Fields.Sections)
            };

            if (item.Fields.Content?.Content == null)
            {
                return model;
            }

            foreach (var contentItem in item.Fields.Content.Content)
            {
                announcement.ProcessContentNodeTypes(contentItem, model.MainContent.Items);
                contentItem.ProcessListNodeTypes(model.MainContent.Items);
                announcement.ProcessEmbeddedAssetBlockNodeTypes(contentItem, model.MainContent.Items);
                announcement.ProcessEmbeddedEntryInlineNodeTypes(contentItem, model.MainContent.Items);
                announcement.ProcessEmbeddedEntryBlockNodeTypes(contentItem, model.MainContent.Items);
                contentItem.ProcessTableNodeTypes(model.MainContent.Items);
            }

            return model;
        }

        public class AnnouncementContent
        {
            public AnnouncementContent()
            {
                Items = new List<ContentItem>();
            }

            public string Id { get; set; }
            public string Headline { get; set; }
            public string Slug { get; set; }
            public string Intro { get; set; }
            public DateTime? PublishedDate { get; set; }
            public DateTime? UpdatedDate { get; set; }
            public string Audience { get; set; }
            public string Category { get; set; }
            public List<ContentItem> Items { get; set; }
        }
    }
}
