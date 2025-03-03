using SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetEmployerProfileUser;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Models
{
    public class EmployerProfilesUser
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }

        public static implicit operator EmployerProfilesUser(GetEmployerProfileUserResult source)
        {
            return new EmployerProfilesUser
            {
                Id = source.Id,
                Email = source.Email,
                FirstName = source.FirstName,
                LastName = source.LastName,
                DisplayName = source.DisplayName
            };
        }
    }
}
