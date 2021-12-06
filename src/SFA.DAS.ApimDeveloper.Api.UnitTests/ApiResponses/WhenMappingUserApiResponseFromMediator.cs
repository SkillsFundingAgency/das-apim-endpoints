using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.Users.Queries;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.ApiResponses
{
    public class WhenMappingUserApiResponseFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(AuthenticateUserQueryResult source)
        {
            var actual = (UserApiResponse)source;

            actual.User.Should().BeEquivalentTo(source.User);
        }
    }
}