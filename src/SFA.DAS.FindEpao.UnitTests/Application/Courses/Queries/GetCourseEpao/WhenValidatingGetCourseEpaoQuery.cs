using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindEpao.Application.Courses.Queries.GetCourseEpao;

namespace SFA.DAS.FindEpao.UnitTests.Application.Courses.Queries.GetCourseEpao
{
    public class WhenValidatingGetCourseEpaoQuery
    {
        [Test, AutoData]
        public async Task And_Not_Match_Regex_Then_Not_Valid(
            GetCourseEpaoQuery query,
            GetCourseEpaoQueryValidator validator)
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
            GetCourseEpaoQuery query,
            GetCourseEpaoQueryValidator validator)
        {
            query.EpaoId = epaoId;

            var validationResult = await validator.ValidateAsync(query);

            validationResult.IsValid().Should().Be(isValid);
        }
    }
}