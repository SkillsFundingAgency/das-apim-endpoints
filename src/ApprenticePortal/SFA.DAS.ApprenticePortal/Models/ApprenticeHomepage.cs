using System;

namespace SFA.DAS.ApprenticePortal.Models
{
    public class ApprenticeHomepage
    {
        public Apprentice Apprentice { get; set; }
        public Apprenticeship Apprenticeship { get; set; }
        public CurrentApprenticeship CurrentApprenticeship { get; set; }
    }
}
