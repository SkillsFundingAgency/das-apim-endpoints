using System;
using System.Collections.Generic;
using System.Text;
using SFA.DAS.Campaign.ExternalApi.Responses;

namespace SFA.DAS.Campaign.Models
{
    public class CardPageModel : PageModel
    {
       public CardLandingPageModel LandingPage { get; set; }
    }

    public class CardLandingPageModel
    {
        public string Slug { get; set; }
        public string Title { get; set; }
        public string Hub { get; set; }
    }
}
