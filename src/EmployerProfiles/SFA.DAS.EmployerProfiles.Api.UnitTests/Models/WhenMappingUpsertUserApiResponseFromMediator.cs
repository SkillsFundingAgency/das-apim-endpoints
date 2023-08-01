using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerProfiles.Api.Models;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerProfiles.Api.UnitTests.Models
{
    public class WhenMappingUpsertUserApiResponseFromMediator
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(EmployerProfile source)
        {
            var actual = (UpsertUserApiResponse) source;
            
            actual.FirstName.Should().BeEquivalentTo(source.FirstName);
            actual.LastName.Should().BeEquivalentTo(source.LastName);
            actual.UserId.Should().BeEquivalentTo(source.UserId);
            actual.Email.Should().Be(source.Email);
        }
        
        
        [Test]
        public void Then_If_Null_Then_Empty_Returned()
        {
            var actual = (UpsertUserApiResponse) (EmployerProfile)null;
            
            actual.FirstName.Should().BeNull();
            actual.LastName.Should().BeNull();
            actual.Email.Should().BeNull();
            actual.UserId.Should().BeNull();
        }
    }
}