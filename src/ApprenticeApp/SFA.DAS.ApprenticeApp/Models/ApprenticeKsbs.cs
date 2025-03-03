using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
   public class ApprenticeKsbs
    {
        public List<Ksb> AllKsbs { get; set; }
        public List<ApprenticeKsbProgressData> KsbProgresses { get; set; }
    }
}
