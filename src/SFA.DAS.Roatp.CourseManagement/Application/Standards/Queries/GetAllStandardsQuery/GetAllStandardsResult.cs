using System.Collections.Generic;
using SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetStandardQuery;

namespace SFA.DAS.Roatp.CourseManagement.Application.Standards.Queries.GetAllStandardsQuery
{
    public class GetAllStandardsResult
    {
       public List<GetStandardResult> Standards { get; set; }
    }
}