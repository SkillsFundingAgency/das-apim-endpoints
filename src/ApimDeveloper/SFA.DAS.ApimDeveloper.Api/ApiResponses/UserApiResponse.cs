using System;
using SFA.DAS.ApimDeveloper.Application.Users.Queries.GetUser;

namespace SFA.DAS.ApimDeveloper.Api.ApiResponses
{
    public class UserApiResponse
    {
        public UserApiResponseItem User { get; set; }
        
        public static implicit operator UserApiResponse(GetUserQueryResult source)
        {
            if (source.User == null)
            {
                return null;
            }
                
            return new UserApiResponse
            {
                User = new UserApiResponseItem
                {
                    Id = source.User.Id,
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
        public Guid Id { get ; set ; }
    }
}