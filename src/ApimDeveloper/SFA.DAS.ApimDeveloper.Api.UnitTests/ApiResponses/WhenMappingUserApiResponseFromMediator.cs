using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApimDeveloper.Api.ApiResponses;
using SFA.DAS.ApimDeveloper.Application.Users.Queries.GetUser;

namespace SFA.DAS.ApimDeveloper.Api.UnitTests.ApiResponses
{
    public class WhenMappingUserApiResponseFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetUserQueryResult source)
        {
            var actual = (UserApiResponse)source;

            actual.User.Should().BeEquivalentTo(source.User);
        }

        [Test]
        public void Then_If_Null_Then_Null_Returned()
        {
            var source = new GetUserQueryResult
            {
                User = null
            };

            var actual = (UserApiResponse)source;

            actual.Should().BeNull();
        }
    }
}