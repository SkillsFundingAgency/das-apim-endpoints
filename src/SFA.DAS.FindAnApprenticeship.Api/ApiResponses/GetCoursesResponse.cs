using System.Collections.Generic;
using System.Linq;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetCourses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.ApiResponses
{
    public class GetCoursesResponse
    {
        public List<GetCourseItem> Courses { get; set; }

        public static implicit operator GetCoursesResponse(GetCoursesQueryResult source)
        {
            return new GetCoursesResponse
            {
                Courses = source.Standards.Select(c=>(GetCourseItem)c).ToList() 
            };
        }
    }

    public class GetCourseItem
    {
        public int LarsCode { get; set; }
        public string Title { get; set; }
        public int Level { get; set; }
        public string Route { get; set; }

        public static implicit operator GetCourseItem(GetStandardsListItem source)
        {
            return new GetCourseItem
            {
                Level = source.Level,
                Route = source.Route,
                Title = source.Title,
                LarsCode = source.LarsCode
            };
        }
    }
}