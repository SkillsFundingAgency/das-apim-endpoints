using System.Collections.Generic;
using System.Linq;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class GetEpaosApiModel
    {
        public IEnumerable<EpaoListItem> Epaos { get; set; }
        public IEnumerable<Link> Links => BuildLinks();

        public static implicit operator GetEpaosApiModel(GetEpaosResult source)
        {
            return new GetEpaosApiModel
            {
                Epaos = source.Epaos.Select(item => (EpaoListItem)item)
            };
        }

        private IEnumerable<Link> BuildLinks()
        {
            return new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = "/epaos"
                }
            };
        }
    }
}