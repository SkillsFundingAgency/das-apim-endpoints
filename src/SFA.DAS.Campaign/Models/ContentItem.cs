using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Campaign.Models
{
    public class ContentItem
    {
        public List<string> Values { get; set; }
        public string Type { get; set; }
        public List<List<string>> TableValue { get; set; }
        public ResourceItem EmbeddedResource { get; set; }
    }

}
