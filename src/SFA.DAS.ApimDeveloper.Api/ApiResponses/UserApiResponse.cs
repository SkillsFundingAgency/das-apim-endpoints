using System;
using SFA.DAS.ApimDeveloper.Application.Users.Queries;

namespace SFA.DAS.ApimDeveloper.Api.ApiResponses
{
    public class UserApiResponse
    {
        public UserApiResponseItem User { get; set; }

        public static implicit operator UserApiResponse(AuthenticateUserQueryResult source)
        {
            return new UserApiResponse
            {
                User = new UserApiResponseItem
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

    public class UserApiResponseItem
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string State { get; set; }
        public string Email { get; set; }
        public bool Authenticated { get; set; }
        public Guid Id { get ; set ; }
    }
}