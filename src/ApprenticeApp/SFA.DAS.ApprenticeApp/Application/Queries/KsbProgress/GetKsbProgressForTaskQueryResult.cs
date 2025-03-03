using SFA.DAS.ApprenticeApp.Models;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.Application.Queries.KsbProgress
{
    public class GetKsbProgressForTaskQueryResult
    {
        public List<ApprenticeKsbProgressData> KSBProgress { get; set; }
    }
}
