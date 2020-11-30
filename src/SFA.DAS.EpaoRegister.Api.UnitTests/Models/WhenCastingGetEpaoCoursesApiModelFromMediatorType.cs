using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models
{
    public class WhenCastingGetEpaoCoursesApiModelFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetEpaoCoursesResult source)
        {
            var response = (GetEpaoCoursesApiModel)source;

            response.EpaoId.Should().BeEquivalentTo(source.EpaoId);
            response.Courses.Count().Should().Be(source.Courses.Count);
        }

        [Test, AutoData]
        public void Then_Creates_Links(
            GetEpaoCoursesResult source)
        {
            var expectedLinks = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = $"/epaos{source.EpaoId}/courses"
                },
                new Link
                {
                    Rel = "epao",
                    Href = $"/epaos{source.EpaoId}"
                }
            };

            var response = (GetEpaoCoursesApiModel) source;

            response.Links.Should().BeEquivalentTo(expectedLinks);
        }

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (GetEpaoCoursesApiModel)null;

            response.Should().BeNull();
        }
    }
}