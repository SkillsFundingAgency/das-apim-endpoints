using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;

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

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (GetEpaoCoursesApiModel)null;

            response.Should().BeNull();
        }
    }
}