using System.Collections.Generic;
using MediatR;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetSectors
{
    public class GetSectorsQuery : IRequest<GetSectorsQueryResult>
    {
        public IEnumerable<ReferenceDataItem> ReferenceDataItems { get; set; }
    }
}
