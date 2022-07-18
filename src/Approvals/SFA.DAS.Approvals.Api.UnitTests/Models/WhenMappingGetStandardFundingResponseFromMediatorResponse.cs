using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetStandardFundingResponseFromMediatorResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(ApprenticeshipFunding source)
        {
            //Act
            var actual = (GetStandardFundingResponse)source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}