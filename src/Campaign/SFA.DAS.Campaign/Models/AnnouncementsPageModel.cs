using System;
using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class AnnouncementsPageModel
    {
        public List<AnnouncementSummary> Announcements { get; set; }

        public AnnouncementsPageModel Build(CmsContent announcements)
        {
            if (announcements.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            return new AnnouncementsPageModel
            {
                Announcements = announcements.Items
                    .Where(item => item?.Fields != null)
                    .Select(item => new AnnouncementSummary
                    {
                        Id = item.Sys?.Id,
                        Headline = item.Fields.Headline,
                        Slug = item.Fields.Slug,
                        Intro = item.Fields.Intro,
                        PublishedDate = item.Fields.Date,
                        Audience = item.Fields.Audience,
                        Category = announcements.GetAnnouncementCategoryTitle(item.Fields.Category)
                    })
                    .OrderByDescending(item => item.PublishedDate ?? DateTime.MinValue)
                    .ToList()
            };
        }

        public class AnnouncementSummary
        {
            public string Id { get; set; }
            public string Headline { get; set; }
            public string Slug { get; set; }
            public string Intro { get; set; }
            public DateTime? PublishedDate { get; set; }
            public string Audience { get; set; }
            public string Category { get; set; }
        }
    }
}
