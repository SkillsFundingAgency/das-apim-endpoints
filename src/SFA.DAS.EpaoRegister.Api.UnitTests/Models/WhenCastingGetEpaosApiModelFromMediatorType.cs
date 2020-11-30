using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models
{
    public class WhenCastingGetEpaosApiModelFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetEpaosResult source)
        {
            var response = (GetEpaosApiModel)source;

            response.Epaos.Count().Should().Be(source.Epaos.Count());
        }

        [Test, AutoData]
        public void Then_Creates_Links(
            GetEpaosResult source)
        {
            var expectedLinks = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = "/epaos"
                }
            };

            var response = (GetEpaosApiModel) source;

            response.Links.Should().BeEquivalentTo(expectedLinks);
        }

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (GetEpaosApiModel)null;

            response.Should().BeNull();
        }
    }
}