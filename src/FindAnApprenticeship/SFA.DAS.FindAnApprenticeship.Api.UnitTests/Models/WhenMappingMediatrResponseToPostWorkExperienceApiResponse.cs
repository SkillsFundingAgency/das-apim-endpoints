using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateWorkExperience;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingMediatrResponseToPostWorkExperienceApiResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(CreateWorkCommandResponse source)
        {
            var actual = (PostWorkExperienceApiResponse)source;

            actual.Should().BeEquivalentTo(source);
        }
    }
}
