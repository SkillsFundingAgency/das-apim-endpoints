using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser
{
    public class GetEmployerProfileUserQueryHandler : IRequestHandler<GetEmployerProfileUserQuery, GetEmployerProfileUserResult>
    {
        private readonly IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> _employerProfilesApiClient;

        public GetEmployerProfileUserQueryHandler(IEmployerProfilesApiClient<EmployerProfilesApiConfiguration> employerProfilesApiClient)
        {
            _employerProfilesApiClient = employerProfilesApiClient;
        }

        public async Task<GetEmployerProfileUserResult> Handle(GetEmployerProfileUserQuery request, CancellationToken cancellationToken)
        {
            var userResponse = await _employerProfilesApiClient.GetWithResponseCode<EmployerProfileUsersApiResponse>(
                new GetEmployerUserAccountRequest(request.UserId.ToString()));

            userResponse.EnsureSuccessStatusCode();

            return new GetEmployerProfileUserResult
            {
                Id = userResponse.Body.Id,
                Email = userResponse.Body.Email,
                FirstName = userResponse.Body.FirstName,
                LastName = userResponse.Body.LastName,
                DisplayName = userResponse.Body.DisplayName,
            };
        }
    }
}