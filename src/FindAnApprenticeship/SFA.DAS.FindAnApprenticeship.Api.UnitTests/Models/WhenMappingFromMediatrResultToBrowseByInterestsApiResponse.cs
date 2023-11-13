using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.BrowseByInterests;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingFromMediatrResultToBrowseByInterestsApiResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(BrowseByInterestsResult source)
        {
            var actual = (BrowseByInterestsApiResponse)source;

            actual.Should().BeEquivalentTo(source);
        }
    }
}