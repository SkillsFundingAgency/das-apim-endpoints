using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Validation;

namespace SFA.DAS.SharedOuterApi.UnitTests.Validation
{
    public class WhenValidatingEpaoId
    {
        [Test, AutoData]
        public async Task And_Not_Match_Regex_Then_Not_Valid(
            string epaoId,
            TestValidator validator)
        {
            var validationResult = await validator.ValidateAsync(epaoId);

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
            TestValidator validator)
        {
            var validationResult = await validator.ValidateAsync(epaoId);

            validationResult.IsValid().Should().Be(isValid);
        }
    }

    public class TestValidator : EpaoIdValidator, IValidator<string>
    {
        public Task<ValidationResult> ValidateAsync(string item)
        {
            var result = new ValidationResult();
            ValidateEpaoId(item, ref result);
            return Task.FromResult(result);
        }
    }
}