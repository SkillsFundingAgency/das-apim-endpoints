using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class BannerPageModel
    {
        public IEnumerable<BannerPageContent> MainContent { get; set; }

        public BannerPageModel Build(CmsContent banner)
        {
            if (banner.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var banners = IterateBannerItems(banner);

            return GenerateHubPageModel(banners);
        }

        private static IEnumerable<BannerPageContent> IterateBannerItems(CmsContent banner)
        {
            var banners = new List<BannerPageContent>();

            foreach (var item in banner.Items)
            {
                var bannerModel = ProcessBanner(item);
                var contentItems = new List<ContentItem>();

                foreach (var contentItem in item.Fields.Content.Content)
                {
                    banner.ProcessContentNodeTypes(contentItem, contentItems);
                    contentItem.ProcessListNodeTypes(contentItems);
                    banner.ProcessEmbeddedAssetBlockNodeTypes(contentItem, contentItems);
                }

                bannerModel.Items = contentItems;

                banners.Add(bannerModel);
            }

            return banners;
        }


        private static BannerPageContent ProcessBanner(Item item)
        {
            var banner = new BannerPageContent
            {
                AllowUserToHideTheBanner = item.Fields.AllowUserToHideTheBanner,
                BackgroundColour = item.Fields.BackgroundColour,
                ShowOnTheHomepageOnly = item.Fields.ShowOnTheHomepageOnly,
                Title = item.Fields.Title,
                Id = item.Sys.Id
            };
            
            return banner;
        }

        private static BannerPageModel GenerateHubPageModel(IEnumerable<BannerPageContent> content)
        {
            return new BannerPageModel()
            {
                MainContent = content
            };
        }

        public class BannerPageContent
        {
            public BannerPageContent()
            {
                Items = new List<ContentItem>();
            }
            public List<ContentItem> Items { get; set; }
            public string BackgroundColour { get; set; }
            public bool AllowUserToHideTheBanner { get; set; }
            public bool ShowOnTheHomepageOnly { get; set; }
            public string Title { get; set; }
            public string Id { get; set; }
        }
    }
}
