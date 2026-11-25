using System.Collections.Generic;

namespace SFA.DAS.Campaign.Models
{
    public class ContentItem
    {
        public List<string> Values { get; set; }
        public string Type { get; set; }
        public List<List<string>> TableValue { get; set; }
        public bool TableHasHeaderRow { get; set; }
        public bool TableHasHeaderColumn { get; set; }
        public ResourceItem EmbeddedResource { get; set; }
        public List<VideoTranscript> VideoTranscripts { get; set; }
        public CtaPanelModel CtaPanel { get; set; }
        public StatsSectionModel StatsSection { get; set; }
    }

    public class VideoTranscript
    {
        public string VideoName { get; set; }
        public string Text { get; set; }
    }

}
