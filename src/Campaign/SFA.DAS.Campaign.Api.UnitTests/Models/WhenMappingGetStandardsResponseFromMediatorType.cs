using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.InnerApi.Responses;

namespace SFA.DAS.Campaign.Api.UnitTests.Models
{
    public class WhenMappingGetStandardsResponseFromMediatorType
    {
        [Test,AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetStandardsListItem source)
        {
            //Act
            var actual = (GetStandardsResponseItem) source;

            //Assert
            actual.LarsCode.Should().Be(source.LarsCode);
        }
    }
}