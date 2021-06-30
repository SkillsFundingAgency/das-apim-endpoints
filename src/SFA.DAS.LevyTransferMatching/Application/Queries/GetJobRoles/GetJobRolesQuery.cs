using System.Collections.Generic;
using MediatR;
using SFA.DAS.LevyTransferMatching.Models.Tags;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetJobRoles
{
    public class GetJobRolesQuery : IRequest<GetJobRolesQueryResult>
    {
        public IEnumerable<ReferenceDataItem> Tags { get; set; }
    }
}
