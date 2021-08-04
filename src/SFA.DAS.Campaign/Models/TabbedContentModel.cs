using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Campaign.Models
{
    public class TabbedContentModel
    {
        public TabbedContentModel()
        {
            Content = new CmsPageModel.PageContent();
        }
        public string Id { get; set; }
        public string TabName { get; set; }
        public string TabTitle { get; set; }
        public bool FindTraineeship { get; set; }
        public CmsPageModel.PageContent Content { get; set; }
    }
}
