namespace SFA.DAS.Campaign.Models
{
    public class HubSectionLinkModel
    {
        public string Id { get; set; }
        public PageType PageType { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Summary { get; set; }
        public string MetaDescription { get; set; }
        public UrlDetails LandingPage { get; set; }
        public CtaPanelModel CtaPanel { get; set; }
    }
}
