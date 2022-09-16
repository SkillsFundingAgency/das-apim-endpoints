namespace SFA.DAS.Campaign.Models
{
    public class PanelPageModel
    {
        public PanelPageModel()
        {
            Content = new CmsPageModel.PageContent();
        }
        public string Title { get; set; }
        public CmsPageModel.PageContent Content { get; set; }
    }
}
