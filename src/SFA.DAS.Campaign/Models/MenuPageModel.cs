using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class MenuPageModel
    {
        public MenuPageContent MainContent { get; set; }

        public MenuPageModel Build(CmsContent menu)
        {
            if (menu.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var menuItems = ProcessMenuItems(menu);

            return GenerateHubPageModel(SetMenuDetails(menuItems));
        }

        private static List<UrlDetails> SetMenuDetails(List<PageModel> menuItems)
        {
            return menuItems.Select(item => new UrlDetails
            {
                Hub = item?.HubType, Title = item?.Title, Slug = item?.Slug,
                PageType = item?.PageType.ToString()
            }).ToList();
        }

        private static List<PageModel> ProcessMenuItems(CmsContent hub)
        {
            

            var cards = hub.Includes?.Entry != null
                ? hub
                    .Includes
                    .Entry.Where(c => c.Sys?.ContentType?.Sys?.Type != null
                                      && c.Sys.ContentType.Sys.Type.Equals("link",
                                          StringComparison.CurrentCultureIgnoreCase)
                                      && c.Sys.ContentType.Sys.LinkType.Equals("ContentType",
                                          StringComparison.CurrentCultureIgnoreCase)
                                      && Enum.TryParse<PageType>(c.Sys.ContentType.Sys.Id, true, out var type) &&
                                      hub.Items[0].Fields.MenuItems.FirstOrDefault(o => o.Sys.Id == c.Sys.Id) != null
                    )
                    .Select(entry => new PageModel
                    {
                        Id = entry.Sys.Id,
                        Slug = entry.Fields.Slug,
                        Summary = entry.Fields.Summary,
                        Title = entry.Fields.Title,
                        HubType = entry.Fields.HubType,
                        MetaDescription = entry.Fields.MetaDescription,
                        PageType = entry.Sys.ContentType.Sys.Id.GetPageType()
                    })
                    .ToList()
                : new List<PageModel>();

            if (!cards.Any())
            {
                return cards;
            }

            for (var i = 0; i < hub.Items[0].Fields.MenuItems.Count; i++)
            {
                cards = cards.OrderBy(o => o.Id == hub.Items[0].Fields.MenuItems[i].Sys.Id).ToList();
            }

            return cards;
        }

        private static string GetPageType(string sysId)
        {
            Enum.TryParse<PageType>(sysId, true, out var pageTypeResult);

            return pageTypeResult.ToString();
        }

        private static MenuPageModel GenerateHubPageModel(List<UrlDetails> pages)
        {
            return new MenuPageModel()
            {
                MainContent = new MenuPageContent
                {
                    Items = pages
                }
            };
        }

        public class MenuPageContent
        {
            public MenuPageContent()
            {
                Items = new List<UrlDetails>();
            }
            public List<UrlDetails> Items { get; set; }
        }
    }
}
