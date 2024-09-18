using System.Collections.Generic;
using SFA.DAS.ApprenticeApp.Models;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Details
{
    public class GetTaskCategoriesQueryResult
    {
        public ApprenticeTaskCategoriesCollection TaskCategories { get; set; }
    }
}