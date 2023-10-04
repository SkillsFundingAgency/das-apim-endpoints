using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingFromMediatrResultToSearchApprenticeshipsApiResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(SearchApprenticeshipsResult source)
        {
            var actual = (SearchApprenticeshipsApiResponse)source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}