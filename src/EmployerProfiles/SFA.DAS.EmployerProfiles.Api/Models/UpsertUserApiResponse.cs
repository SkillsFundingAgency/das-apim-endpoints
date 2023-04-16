using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerProfiles.Api.Models
{
    public class UpsertUserApiResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public static implicit operator UpsertUserApiResponse(EmployerProfile source)
        {
            return new UpsertUserApiResponse
            {
                FirstName = source?.FirstName,
                LastName = source?.LastName,
                Email = source?.Email,
                UserId = source?.UserId
            };
        }
    }
}
