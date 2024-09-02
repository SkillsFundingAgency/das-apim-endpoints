using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeTaskModelData
    {
        public ApprenticeTask Task { get; set; }
        public ApprenticeTaskCategoriesCollection TaskCategories { get; set; }
        //public List<ApprenticeKsbProgressData> KSBProgress { get; set; }

        public List<ApprenticeKsbData>? KSBProgress { get; set; }
    }
}
