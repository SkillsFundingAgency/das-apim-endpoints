using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EpaoRegister.Api.Infrastructure;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class GetEpaoCoursesApiModel
    {
        public string EpaoId { get; set; }
        public IEnumerable<EpaoCourse> Courses { get; set; }
        public IEnumerable<Link> Links { get; private set; }

        public static explicit operator GetEpaoCoursesApiModel(GetEpaoCoursesResult source)
        {
            return new GetEpaoCoursesApiModel
            {
                EpaoId = source.EpaoId,
                Courses = source.Courses.Select(response => (EpaoCourse)response)
            };
        }

        public void BuildLinks(IUrlHelper urlHelper)
        {
            Links = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = urlHelper.RouteUrl(RouteNames.GetEpaoCourses, new {EpaoId}, ProtocolNames.Https)
                },
                new Link
                {
                    Rel = "epao",
                    Href = urlHelper.RouteUrl(RouteNames.GetEpao, new {EpaoId}, ProtocolNames.Https)
                }
            };
        }
    }
}