using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;
using System.Collections.Generic;
using static SFA.DAS.Campaign.Models.MenuPageModel;

namespace SFA.DAS.Campaign.Models
{
    public class PanelPageModel
    {
        public IEnumerable<PanelPageContent> MainContent { get; set; }

        public class PanelPageContent
        {
            public PanelPageContent()
            {
                Items = new List<ContentItem>();
            }
            public List<ContentItem> Items { get; set; }
            public string Slug { get; set; }
            public string ButtonText { get; set; }
            public string ButtonUrl { get; set; }
            public List<string> ButtonStyle { get; set; }
            public string Title { get; set; }
            public string Id { get; set; }
        }

        public PanelPageModel Build(CmsContent panel, MenuPageContent content)
        {
            if (panel.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var panels = new List<PanelPageContent>();
            var panelItem = panel.Items;
            foreach (var item in panelItem)
            {
                var panelModel = new PanelPageContent
                {
                    Slug = item.Fields.Slug,
                    Title = item.Fields.Title,
                    Id = item.Sys.Id,
                    ButtonText = item.Fields.ButtonText,
                    ButtonUrl = item.Fields.ButtonUrl,
                    ButtonStyle = item.Fields.ButtonStyle
                };
                panels.Add(panelModel);
            }


            return GenerateModelPage(panels);
        }

        public PanelPageModel GenerateModelPage(IEnumerable<PanelPageContent> content)
        {
            return new PanelPageModel()
            {
                MainContent = content
            };
        }
    }
}
