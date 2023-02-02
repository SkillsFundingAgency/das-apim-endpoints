using SFA.DAS.Funding.Models;
using System.Collections.Generic;

namespace SFA.DAS.Funding.Api.Models
{
    public class GetApprenticeshipsResponse
    {
        public IEnumerable<ApprenticeshipDto> Apprenticeships { get; set; }

        public GetApprenticeshipsResponse(List<Apprenticeship> source)
        {
            List<ApprenticeshipDto> apprenticeships = new();
            foreach (var apprenticeship in source)
            {
                apprenticeships.Add(new ApprenticeshipDto()
                {
                    Uln = apprenticeship.Uln,
                    FirstName = apprenticeship.FirstName,
                    LastName = apprenticeship.LastName
                });
            }

            Apprenticeships = apprenticeships;
        }
    }
}
