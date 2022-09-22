using Microsoft.AspNetCore.Mvc.RazorPages;
using SFA.DAS.Campaign.Extensions;
using SFA.DAS.Campaign.ExternalApi.Responses;
using System.Collections.Generic;
using static SFA.DAS.Campaign.Models.CmsPageModel;

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
            public ResourceItem PanelImage { get; set; }
        }

        public PanelPageModel Build(CmsContent panel)
        {
            if (panel.ContentItemsAreNullOrEmpty())
            {
                return null;
            }

            var panels = new List<PanelPageContent>();
            foreach (var item in panel.Items)
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

                foreach (var contentItem in item.Fields.Content.Content)
                {
                    contentItem.ProcessListNodeTypes(panelModel.Items);
                    contentItem.ProcessContentNodeTypes(panelModel.Items);
                }

                if (item.Fields.Image != null && !string.IsNullOrWhiteSpace(item.Fields.Image.Sys.Id))
                {
                    panelModel.PanelImage = panel.GetEmbeddedResource(item.Fields.Image.Sys.Id);                    
                }

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
