using System.Collections.Generic;
using MediatR;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLevels
{
    public class GetLevelsQuery : IRequest<GetLevelsQueryResult>
    {
        public IEnumerable<ReferenceDataItem> ReferenceDataItems { get; set; }
    }
}
