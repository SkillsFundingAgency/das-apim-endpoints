using System;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.AuthenticateUser;

namespace SFA.DAS.ApimDeveloper.Api.ApiResponses
{
    public class UserAuthenticationApiResponse
    {
        public UserAuthenticationApiResponseItem User { get; set; }

        public static implicit operator UserAuthenticationApiResponse(AuthenticateUserCommandResult source)
        {
            if (source.User == null)
            {
                return null;
            }
                
            return new UserAuthenticationApiResponse
            {
                User = new UserAuthenticationApiResponseItem
                {
                    Id = source.User.Id,
                    Authenticated = source.User.Authenticated,
                    Email = source.User.Email,
                    FirstName = source.User.FirstName,
                    LastName = source.User.LastName,
                    State = source.User.State
                }
            };
        }
    }

    public class UserAuthenticationApiResponseItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public bool Authenticated { get; set; }
        public Guid Id { get ; set ; }
    }
}