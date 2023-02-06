using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Application.Queries.GetApprenticeships
{
    public class GetApprenticeshipsHandler : IRequestHandler<GetApprenticeshipsQuery, GetApprenticeshipsResult>
    {
        private readonly IApprenticeshipsService _apprenticeshipsService;

        public GetApprenticeshipsHandler(IApprenticeshipsService apprenticeshipsService)
        {
            _apprenticeshipsService = apprenticeshipsService;
        }

        public async Task<GetApprenticeshipsResult> Handle(GetApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeships = await _apprenticeshipsService.GetAll(request.Ukprn);

            var apprenticeshipsToReturn = MapApprenticeships(apprenticeships);

            return new GetApprenticeshipsResult
            {
                Apprenticeships = apprenticeshipsToReturn
            };
        }

        private static List<Apprenticeship> MapApprenticeships(IEnumerable<ApprenticeshipDto> apprenticeshipsDto)
        {
            var result = apprenticeshipsDto
                .Select(x => new Apprenticeship { Uln = x.Uln, FirstName = x.FirstName, LastName = x.LastName })
                .ToList();
            return result;
        }
    }
}