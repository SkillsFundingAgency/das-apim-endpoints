using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.DeleteVolunteeringOrWorkExperience;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromMediatorResponseToDeleteVolunteeringOrWorkExperienceApiResponse
{
    [Test, MoqAutoData]
    public async Task Then_Fields_Are_Mapped_Correctly(GetDeleteVolunteeringOrWorkExperienceQueryResult source)
    {
        var result = (GetDeleteVolunteeringOrWorkHistoryApiResponse)source;

        using (new AssertionScope())
        {
            result.Id.Should().Be(source.Id);
            result.ApplicationId.Should().Be(source.ApplicationId);
            result.Organisation.Should().Be(source.Organisation);
            result.Description.Should().Be(source.Description);
            result.FromDate.Should().Be(source.FromDate);
            result.ToDate.Should().Be(source.ToDate);
        }
    }
}
