using System.Collections.Generic;
using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses
{
    public class GetEpaoCoursesResult
    {
        public IReadOnlyList<EpaoCourse> EpaoCourses { get; set; }
    }
}