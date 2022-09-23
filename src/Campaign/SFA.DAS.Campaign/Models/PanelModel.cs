using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Campaign.Models
{
    public class PanelModel
    {
        public PanelContent MainContent { get; set; }
        public PageModel PageAttributes { get; set; }

        public PanelModel Build(CmsContent cmsPanel)
        {
            if (cmsPanel.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var panelItem = cmsPanel.Items.FirstOrDefault();

            var panelContent = new PanelModel
            {
                PageAttributes = new PageModel
                {
                    Slug = panelItem.Fields.Slug,
                    Title = panelItem.Fields.Title,
                    Id = panelItem.Sys.Id,
                },
                MainContent = new PanelContent
                {
                    Button = new ButtonModel
                    {
                        Title = panelItem.Fields.ButtonText,
                        Url = panelItem.Fields.ButtonUrl,
                        Styles = panelItem.Fields.ButtonStyle
                    }
                }
            };

            foreach (var contentItem in panelItem.Fields.Content.Content)
            {
                contentItem.ProcessListNodeTypes(panelContent.MainContent.Items);
                contentItem.ProcessContentNodeTypes(panelContent.MainContent.Items);
            }

            if (panelItem.Fields.Image != null && !string.IsNullOrWhiteSpace(panelItem.Fields.Image.Sys.Id))
            {
                panelContent.MainContent.Image = cmsPanel.GetEmbeddedResource(panelItem.Fields.Image.Sys.Id);
            }

            return panelContent;
        }

        public class PanelContent
        {
            public PanelContent()
            {
                Items = new List<ContentItem>();
            }

            public List<ContentItem> Items { get; set; }
            public ResourceItem Image { get; set; }
            public ButtonModel Button { get; set; }
        }
    }
}
