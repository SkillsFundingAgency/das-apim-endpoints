using MediatR;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetOrganisationName
{
    public class GetOrganisationNameQuery : IRequest<GetOrganisationNameQueryResult>
    {
        public string EncodedAccountId { get; set; }
    }
}
