using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.Users.Commands.AuthenticateUser;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.ApiResponses
{
    public class WhenMappingAuthenticateUserApiResponseFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(AuthenticateUserCommandResult source)
        {
            var actual = (UserAuthenticationApiResponse)source;

            actual.User.Should().BeEquivalentTo(source.User);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            var source = new AuthenticateUserCommandResult
            {
                User = null
            };

            var actual = (UserAuthenticationApiResponse)source;

            actual.Should().BeNull();
        }
    }
}