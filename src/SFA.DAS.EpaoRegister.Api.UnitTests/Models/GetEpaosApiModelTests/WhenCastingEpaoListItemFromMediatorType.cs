using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models.GetEpaosApiModelTests
{
    public class WhenCastingEpaoListItemFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetEpaosListItem source)
        {
            var response = (EpaoListItem)source;

            response.Should().BeEquivalentTo(source, options => 
                options.ExcludingMissingMembers());
        }

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (EpaoListItem)null;

            response.Should().BeNull();
        }
    }
}