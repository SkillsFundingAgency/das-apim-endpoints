using System.Collections.Generic;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class GetEpaosApiModel
    {
        public IEnumerable<EpaoListItem> Epaos { get; set; }
    }
}