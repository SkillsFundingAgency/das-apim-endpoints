using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.Entities;
using SFA.DAS.ApprenticeAan.Application.Profiles.Queries.GetProfilesByUserType;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.Profiles.Queries.GetProfiles
{
    public class GetProfilesByUserTypeQueryResultTests
    {
        [Test]
        [MoqAutoData]
        public void Result_PopulatesGetRegionResult(List<ProfileModel> profiles)
        {
            var result = (GetProfilesByUserTypeQueryResult)profiles;

            result.ProfileModels.Should().BeEquivalentTo(profiles);
        }
    }
}