using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
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