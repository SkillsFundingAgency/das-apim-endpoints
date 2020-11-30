using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetFrameworkFundingResponseFromMediatorResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetFrameworksListItem.FundingPeriod source)
        {
            //Act
            var actual = (GetFrameworkFundingResponse)source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}