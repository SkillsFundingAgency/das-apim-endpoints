using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EpaoRegister.Api.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class EpaoListItem
    {
        public string Id { get; set; }
        public uint? Ukprn { get; set; }
        public string Name { get; set; }
        public IEnumerable<Link> Links { get; private set; }

        public static implicit operator EpaoListItem(InnerApi.Responses.GetEpaosListItem source)
        {
            return new EpaoListItem
            {
                Id = source.Id,
                Ukprn = source.Ukprn,
                Name = source.Name
            };
        }

        public void BuildLinks(IUrlHelper urlHelper)
        {
            Links = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = urlHelper.RouteUrl(RouteNames.GetEpao, new {EpaoId = Id}, ProtocolNames.Https)
                },
                new Link
                {
                    Rel = "courses",
                    Href = urlHelper.RouteUrl(RouteNames.GetEpaoCourses, new {EpaoId = Id}, ProtocolNames.Https)
                }
            };
        }
    }
}
