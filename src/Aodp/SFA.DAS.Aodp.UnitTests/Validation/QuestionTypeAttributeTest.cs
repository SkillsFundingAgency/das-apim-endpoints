using SFA.DAS.Aodp.UnitTests.Validation;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Api.UnitTests.Validation.Attributes
{
    [TestFixture]
    public class QuestionTypeAttributeTests
    {
        private class Model
        {
            [QuestionType]
            public string? Value { get; set; }
        }

        [Test]
        public void Valid_question_type_passes()
        {
            var model = new Model { Value = "Text" }; // replace with real allowed value

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.True);
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void Invalid_question_type_fails()
        {
            var model = new Model { Value = "InvalidValue" };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.False);
            Assert.That(errors, Is.Not.Empty);
        }

        [Test]
        public void Null_value_fails()
        {
            var model = new Model { Value = null };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.False);
            Assert.That(errors, Is.Not.Empty);
        }
    }
}