using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class GetEpaoCoursesApiModel
    {
        public string EpaoId { get; set; }
        public IEnumerable<EpaoCourse> Courses { get; set; }
        public IEnumerable<Link> Links => BuildLinks();

        public static explicit operator GetEpaoCoursesApiModel(GetEpaoCoursesResult source)
        {
            return new GetEpaoCoursesApiModel
            {
                EpaoId = source.EpaoId,
                Courses = source.Courses.Select(response => (EpaoCourse)response)
            };
        }

        private IEnumerable<Link> BuildLinks()
        {
            return new List<Link>
            {
                new Link{}
            };
        }
    }
}