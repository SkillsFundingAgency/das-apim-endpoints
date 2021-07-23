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
        public string TabName { get; set; }
        public string TabTitle { get; set; }
        public CmsPageModel.PageContent Content { get; set; }
    }
}
