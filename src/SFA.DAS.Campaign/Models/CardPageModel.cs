using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.Campaign.Models
{
    public class CardPageModel : PageModel
    {
       public LandingPage LandingPage { get; set; }
    }

    public class LandingPage
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Hub { get; set; }
    }
}
