using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByAccountDetails
{
    public class GetRegistrationsByAccountDetailsQueryHandler : IRequestHandler<GetRegistrationsByAccountDetailsQuery, GetRegistrationsByAccountDetailsQueryResult>
    {
        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _commitmentsApiClient;

        public GetRegistrationsByAccountDetailsQueryHandler(
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apprenticeCommitmentsApiClient) 
        {
            _commitmentsApiClient = apprenticeCommitmentsApiClient;
        }

        public async Task<GetRegistrationsByAccountDetailsQueryResult> Handle(GetRegistrationsByAccountDetailsQuery request, CancellationToken cancellationToken)
        {
            var registration = await _commitmentsApiClient.Get<List<Registration>>(new GetRegistrationsByAccountDetailsRequest(
                request.FirstName, request.LastName, request.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));

            return new GetRegistrationsByAccountDetailsQueryResult { Registrations = registration };
        }
    }
}
