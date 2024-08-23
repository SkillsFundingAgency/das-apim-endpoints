using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
    // ksb data to be returned to pwa as viewmodel
    public class ApprenticeKsbs
    {
        public List<Ksb> AllKsbs { get; set; }
        public List<ApprenticeKsbProgressData> KsbProgresses { get; set; }
    }
}
