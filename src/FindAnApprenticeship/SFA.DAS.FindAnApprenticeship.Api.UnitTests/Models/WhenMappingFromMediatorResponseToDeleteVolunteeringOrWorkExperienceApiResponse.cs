using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.VolunteeringOrWorkExperience.GetWorkExperience;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;
public class WhenMappingFromMediatorResponseToDeleteVolunteeringOrWorkExperienceApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetVolunteeringOrWorkExperienceItemQueryResult source)
    {
        var result = (GetVolunteeringOrWorkExperienceItemApiResponse)source;

        using (new AssertionScope())
        {
            result.Id.Should().Be(source.Id);
            result.ApplicationId.Should().Be(source.ApplicationId);
            result.Organisation.Should().Be(source.Organisation);
            result.Description.Should().Be(source.Description);
            result.StartDate.Should().Be(source.StartDate);
            result.EndDate.Should().Be(source.EndDate);
        }
    }
}
