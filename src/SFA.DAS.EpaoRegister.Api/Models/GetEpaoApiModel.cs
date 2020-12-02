using SFA.DAS.EpaoRegister.InnerApi.Responses;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EpaoRegister.Api.Infrastructure;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class GetEpaoApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint? Ukprn { get; set; }
        public string Email { get; set; }
        public EpaoAddress Address { get; set; }
        public IEnumerable<Link> Links { get; private set; }

        public static explicit operator GetEpaoApiModel(GetEpaoResponse source)
        {
            if (source == null)
                return null;

            return new GetEpaoApiModel
            {
                Id = source.Id,
                Name = source.Name,
                Ukprn = source.Ukprn,
                Email = source.Email,
                Address = (EpaoAddress)source.Address
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
