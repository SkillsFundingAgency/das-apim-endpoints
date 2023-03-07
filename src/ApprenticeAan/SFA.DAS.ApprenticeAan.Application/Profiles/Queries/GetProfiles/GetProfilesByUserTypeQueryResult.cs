using SFA.DAS.ApprenticeAan.Application.Entities;

namespace SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQueryResult
    {
        public List<ProfileModel> ProfileModels { get; set; } = new();

        public static implicit operator GetProfilesByUserTypeQueryResult(List<ProfileModel> profiles) => new()
        {
            ProfileModels = profiles
        };
    }
}