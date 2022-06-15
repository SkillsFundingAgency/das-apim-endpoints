using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetProvidersResponseFromMediatorResponse
    {
        [Test, AutoData]
        public void Then_Maps_The_Fields_Correctly(GetProvidersListItem source)
        {
            //Act
            var actual = (GetProvidersResponse) source;
            
            //Assert
            actual.Should().BeEquivalentTo(source);
        }
    }
}