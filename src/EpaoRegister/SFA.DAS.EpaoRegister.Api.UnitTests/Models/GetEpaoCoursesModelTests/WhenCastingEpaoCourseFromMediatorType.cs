using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models.GetEpaoCoursesModelTests
{
    public class WhenCastingEpaoCourseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetStandardResponse source)
        {
            var response = (EpaoCourse)source;

            response.Id.Should().Be(source.LarsCode);
            response.Periods.Should().BeEquivalentTo(source.StandardDates);
        }

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (EpaoCourse)null;

            response.Should().BeNull();
        }
    }
}