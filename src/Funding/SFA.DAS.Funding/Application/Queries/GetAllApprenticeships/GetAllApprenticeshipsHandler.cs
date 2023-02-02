using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Funding.Application.Queries.GetProviderEarningsSummary;
using SFA.DAS.Funding.InnerApi.Responses;
using SFA.DAS.Funding.Interfaces;
using SFA.DAS.Funding.Models;

namespace SFA.DAS.Funding.Application.Queries.GetAllApprenticeships
{
    public class GetAllApprenticeshipsHandler : IRequestHandler<GetAllApprenticeshipsQuery, GetAllApprenticeshipsResult>
    {
        private readonly IApprenticeshipsService _apprenticeshipsService;

        public GetAllApprenticeshipsHandler(IApprenticeshipsService apprenticeshipsService)
        {
            _apprenticeshipsService = apprenticeshipsService;
        }

        public async Task<GetAllApprenticeshipsResult> Handle(GetAllApprenticeshipsQuery request, CancellationToken cancellationToken)
        {
            var apprenticeships = await _apprenticeshipsService.GetAll(request.Ukprn);

            var apprenticeshipsToReturn = MapApprenticeships(apprenticeships);

            return new GetAllApprenticeshipsResult
            {
                Apprenticeships = apprenticeshipsToReturn
            };
        }

        private static List<Apprenticeship> MapApprenticeships(IEnumerable<ApprenticeshipDto> apprenticeshipsDto)
        {
            List<Apprenticeship> result = new();
            foreach (var apprenticeship in apprenticeshipsDto)
            {
                result.Add(new Apprenticeship() { Uln = apprenticeship.Uln, FirstName= apprenticeship.FirstName,LastName= apprenticeship.LastName });
            }
            return result;
        }
    }
}