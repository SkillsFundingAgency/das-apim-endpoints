using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByEmail
{
    public class GetRegistrationsByEmailQueryHandler : IRequestHandler<GetRegistrationsByEmailQuery, GetRegistrationsByEmailQueryResult>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _commitmentsApiClient;

        public GetRegistrationsByEmailQueryHandler(
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apprenticeCommitmentsApiClient)
        {
            _commitmentsApiClient = apprenticeCommitmentsApiClient;
        }

        public async Task<GetRegistrationsByEmailQueryResult> Handle(GetRegistrationsByEmailQuery request, CancellationToken cancellationToken)
        {
            var registration = await _commitmentsApiClient.Get<List<Registration>>(new GetRegistrationsByEmailRequest(request.Email));
            return new GetRegistrationsByEmailQueryResult { Registrations = registration };
        }
    }
}
