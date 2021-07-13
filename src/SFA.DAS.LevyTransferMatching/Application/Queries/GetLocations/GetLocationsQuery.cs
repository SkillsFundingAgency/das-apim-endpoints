using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations
{
    public class GetLocationsQuery : IRequest<GetLocationsResult>
    {
        public string SearchTerm { get; set; }
    }
}
