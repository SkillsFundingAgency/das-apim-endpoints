using System.Collections.Generic;
using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses
{
    public class GetEpaoCoursesResult
    {
        public string EpaoId { get; set; }
        public IReadOnlyList<GetStandardResponse> Courses { get; set; }
    }
}