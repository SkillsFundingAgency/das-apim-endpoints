using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Assessors.Api.Models;
using SFA.DAS.Assessors.InnerApi.Responses;

namespace SFA.DAS.Assessors.Api.UnitTests.Models
{
    public class WhenCastingGetStandardOptionsResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetStandardOptionsListItem source)
        {
            var response = (GetStandardOptionsItem)source;

            response.Should().BeEquivalentTo(source, options => options.ExcludingMissingMembers());
        }
    }
}
