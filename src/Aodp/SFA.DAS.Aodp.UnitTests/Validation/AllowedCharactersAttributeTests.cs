using NUnit.Framework;
using SFA.DAS.Aodp.UnitTests.Validation;
using SFA.DAS.Aodp.Validation;
using SFA.DAS.Aodp.Validation.Attributes;

namespace SFA.DAS.Aodp.Api.UnitTests.Validation.Attributes
{
    [TestFixture]
    public class AllowedCharactersAttributeTests
    {
        private class Model
        {
            [AllowedCharacters(TextCharacterProfile.FreeText)]
            public string? Value { get; set; }
        }

        [Test]
        public void Valid_text_passes()
        {
            var model = new Model { Value = "Hello world" };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.True);
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void Script_tags_fail()
        {
            var model = new Model { Value = "<script>" };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Null_value_passes()
        {
            var model = new Model { Value = null };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.True);
        }
    }
}