using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Models
{
    public class WhenMappingAccountLegalEntityDtoFromMediatorResult
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(AccountLegalEntity source)
        {
            var actual = (AccountLegalEntityDto) source;
            
            actual.Should().BeEquivalentTo(source, options=>options.Excluding(c=>c.LegalEntityName));
            actual.LegalEntityName.Should().Be(source.LegalEntityName);
        }
    }
}