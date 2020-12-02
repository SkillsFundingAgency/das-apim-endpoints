using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EpaoRegister.Api.Infrastructure;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class GetEpaosApiModel
    {
        public IReadOnlyList<EpaoListItem> Epaos { get; set; }
        public IEnumerable<Link> Links { get; private set; }

        public static implicit operator GetEpaosApiModel(GetEpaosResult source)
        {
            return new GetEpaosApiModel
            {
                Epaos = source.Epaos.Select(item => (EpaoListItem)item).ToList()
            };
        }

        public void BuildLinks(IUrlHelper urlHelper)
        {
            Links = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = urlHelper.RouteUrl(RouteNames.GetEpaos, null, ProtocolNames.Https)
                }
            };

            foreach (var epao in Epaos)
            {
                epao.BuildLinks(urlHelper);
            }
        }
    }
}