using SFA.DAS.ApprenticeApp.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetActiveStandards
{
    public class GetActiveStandardsQueryResult
    {
        public IEnumerable<Courses> Courses { get; set; } = Enumerable.Empty<Courses>();
    }
}
