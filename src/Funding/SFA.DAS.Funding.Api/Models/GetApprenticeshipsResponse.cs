using SFA.DAS.Funding.Models;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.Funding.Api.Models
{
    public class GetApprenticeshipsResponse
    {
        public IEnumerable<ApprenticeshipDto> Apprenticeships { get; set; }

        public GetApprenticeshipsResponse(List<Apprenticeship> source)
        {
            List<ApprenticeshipDto> apprenticeships = new();

            Apprenticeships = source.Select(x => new ApprenticeshipDto { FirstName = x.FirstName, LastName = x.LastName, Uln = x.Uln });
        }
    }
}