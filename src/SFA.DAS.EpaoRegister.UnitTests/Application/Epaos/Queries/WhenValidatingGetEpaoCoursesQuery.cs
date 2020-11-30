using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaoCourses;

namespace SFA.DAS.EpaoRegister.UnitTests.Application.Epaos.Queries
{
    public class WhenValidatingGetEpaoCoursesQuery
    {
        [Test, AutoData]
        public async Task And_Not_Match_Regex_Then_Not_Valid(
            GetEpaoCoursesQuery query,
            GetEpaoCoursesQueryValidator validator)
        {
            var validationResult = await validator.ValidateAsync(query);

            validationResult.IsValid().Should().BeFalse();
            validationResult.ValidationDictionary.Should().ContainKey("epaoId");
        }

        [Test]
        [InlineAutoData(null, false)]
        [InlineAutoData("", false)]
        [InlineAutoData("^%$£&^*", false)]
        [InlineAutoData("EPAO001", false)]
        [InlineAutoData("EPA0001", true)]
        [InlineAutoData("EPA0901", true)]
        [InlineAutoData("epa9999", true)]
        [InlineAutoData("epa999999999", true)]
        [InlineAutoData("epa9999999999", false)]
        public async Task Match_Regex_Cases(
            string epaoId,
            bool isValid,
            GetEpaoCoursesQuery query,
            GetEpaoCoursesQueryValidator validator)
        {
            query.EpaoId = epaoId;

            var validationResult = await validator.ValidateAsync(query);

            validationResult.IsValid().Should().Be(isValid);
        }
    }
}