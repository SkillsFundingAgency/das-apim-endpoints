using System.Collections.Generic;
using MediatR;
using SFA.DAS.LevyTransferMatching.Models.ReferenceData;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles
{
    public class GetJobRolesQuery : IRequest<GetJobRolesQueryResult>
    {
        public IEnumerable<ReferenceDataItem> ReferenceDataItems { get; set; }
    }
}
