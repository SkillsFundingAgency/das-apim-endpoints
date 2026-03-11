using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.ToolsSupport.InnerApi.Requests;
using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.GetUserByUserRef;

public class GetUserByUserRefQueryHandler(IInternalApiClient<EmployerProfilesApiConfiguration> client) : IRequestHandler<GetUserByUserRefQuery, GetUserByUserRefQueryResult>
{
    public async Task<GetUserByUserRefQueryResult> Handle(GetUserByUserRefQuery request, CancellationToken cancellationToken)
    {
        var userResponse = await client.Get<GetUserProfileResponse>(new GetUserProfileRequest(request.UserRef));
        
        if (userResponse == null)
        {
            return new GetUserByUserRefQueryResult { User = null };
        }

        return new GetUserByUserRefQueryResult 
        { 
            User = new UserProfile
            {
                Id = userResponse.Id,
                DisplayName = userResponse.DisplayName,
                FirstName = userResponse.FirstName,
                LastName = userResponse.LastName,
                Email = userResponse.Email,
                GovUkIdentifier = userResponse.GovUkIdentifier,
                IsSuspended = userResponse.IsSuspended,
                IsActive = userResponse.IsActive,
                IsLocked = userResponse.IsLocked
            }
        };
    }
}