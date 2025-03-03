using System;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class ApprenticeTaskCategories
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
    }

    public class ApprenticeTaskCategoriesCollection
    {
        public List<ApprenticeTaskCategories> TaskCategories { get; set; }
    }

}
