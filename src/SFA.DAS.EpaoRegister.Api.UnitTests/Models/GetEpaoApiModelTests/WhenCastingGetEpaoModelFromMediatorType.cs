using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models.GetEpaoApiModelTests
{
    public class WhenCastingGetEpaoModelFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            SearchEpaosListItem source)
        {
            var response = (GetEpaoApiModel)source;

            response.Should().BeEquivalentTo(source);
        }

        [Test, AutoData]
        public void Then_Creates_Links(
            SearchEpaosListItem source)
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

            var response = (GetEpaoApiModel) source;

            response.Links.Should().BeEquivalentTo(expectedLinks);
        }

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (GetEpaoApiModel)null;

            response.Should().BeNull();
        }
    }
}