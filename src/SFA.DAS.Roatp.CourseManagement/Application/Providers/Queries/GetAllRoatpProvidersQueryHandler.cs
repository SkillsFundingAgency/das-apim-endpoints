using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Roatp.CourseManagement.Application.Locations.Queries;
using SFA.DAS.Roatp.CourseManagement.InnerApi.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Roatp.CourseManagement.Application.Providers.Queries
{

    public class GetAllRoatpProvidersQueryHandler : IRequestHandler<GetAllRoatpProvidersQuery, GetAllRoatpProvidersQueryResult>
    {
        private readonly IRoatpServiceApiClient<RoatpApiConfiguration> _apiClient;

        public GetAllRoatpProvidersQueryHandler(IRoatpServiceApiClient<RoatpApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }


        public async Task<GetAllRoatpProvidersQueryResult> Handle(GetAllRoatpProvidersQuery request, CancellationToken cancellationToken)
        {
            var response = await _apiClient.Get<List<RoatpProviderModel>>(request);
            return new GetAllRoatpProvidersQueryResult() { Providers = response };
        }
    }
}
