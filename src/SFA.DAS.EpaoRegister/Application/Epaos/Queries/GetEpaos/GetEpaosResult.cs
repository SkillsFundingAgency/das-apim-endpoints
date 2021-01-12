using System.Collections.Generic;
using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos
{
    public class GetEpaosResult
    {
        public IEnumerable<GetEpaosListItem> Epaos { get; set; }
    }
}