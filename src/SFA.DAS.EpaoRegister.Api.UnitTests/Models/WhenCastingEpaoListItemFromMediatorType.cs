using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models
{
    public class WhenCastingEpaoListItemFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetEpaosListItem source)
        {
            var response = (EpaoListItem)source;

            response.Should().BeEquivalentTo(source);
        }

        [Test, AutoData]
        public void Then_Creates_Links(
            GetEpaosListItem source)
        {
            var expectedLinks = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = $"/epaos{source.Id}"
                },
                new Link
                {
                    Rel = "courses",
                    Href = $"/epaos{source.Id}/courses"
                }
            };

            var response = (EpaoListItem) source;

            response.Links.Should().BeEquivalentTo(expectedLinks);
        }

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (EpaoListItem)null;

            response.Should().BeNull();
        }
    }
}