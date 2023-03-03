using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQueryResult
    {
        public List<ProfileModel> Profiles { get; set; } = new();

        public static implicit operator GetProfilesByUserTypeQueryResult(List<ProfileModel> profiles) => new()
        {
            Profiles = profiles
        };
    }
}