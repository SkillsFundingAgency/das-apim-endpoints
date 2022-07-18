using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetFrameworkResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_The_Fields(GetFrameworksListItem source)
        {
            var actual = (GetFrameworkResponse) source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}