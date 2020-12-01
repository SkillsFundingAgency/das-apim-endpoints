using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetEpaoResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetEpaosListItem source)
        {
            var actual = (GetEpaoResponse) source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}