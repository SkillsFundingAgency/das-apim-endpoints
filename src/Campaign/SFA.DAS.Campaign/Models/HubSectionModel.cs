using System.Collections.Generic;

namespace SFA.DAS.Campaign.Models
{
    public class HubSectionModel
    {
        public string SectionType { get; set; }
        public string Heading { get; set; }
        public string Introduction { get; set; }
        public ContentItem Image { get; set; }
        public List<HubSectionLinkModel> StepperLinks { get; set; }
        public List<HubSectionLinkModel> StandardLinks { get; set; }
        public CtaPanelModel CtaPanel { get; set; }
        public List<StatsSectionModel> StatisticsSections { get; set; }
    }
}
