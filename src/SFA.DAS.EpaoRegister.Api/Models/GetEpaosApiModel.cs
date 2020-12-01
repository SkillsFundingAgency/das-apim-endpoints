using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using SFA.DAS.EpaoRegister.Api.Infrastructure;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class GetEpaosApiModel
    {
        public IEnumerable<EpaoListItem> Epaos { get; set; }
        public IEnumerable<Link> Links { get; private set; }

        public static implicit operator GetEpaosApiModel(GetEpaosResult source)
        {
            return new GetEpaosApiModel
            {
                Epaos = source.Epaos.Select(item => (EpaoListItem)item)
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