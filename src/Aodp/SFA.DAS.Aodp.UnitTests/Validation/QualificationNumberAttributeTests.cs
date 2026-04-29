using SFA.DAS.Aodp.Validation;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.Aodp.Api.UnitTests.Validation.Attributes
{
    [TestFixture]
    public class QualificationNumberAttributeTests
    {
        private class TestModel
        {
            [QualificationNumber]
            public string? Qan { get; set; }
        }

        private static bool Validate(object model, out List<ValidationResult> results)
        {
            var context = new ValidationContext(model);
            results = new List<ValidationResult>();

            return Validator.TryValidateObject(model, context, results, validateAllProperties: true);
        }

        [Test]
        public void Valid_Qualification_Number_Passes()
        {
            var model = new TestModel
            {
                Qan = "12345678"
            };

            var isValid = Validate(model, out var results);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Valid_Qualification_Number_With_Letter_Passes()
        {
            var model = new TestModel
            {
                Qan = "1234567X"
            };

            var isValid = Validate(model, out var results);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Valid_Qualification_Number_With_Slashes_Passes()
        {
            var model = new TestModel
            {
                Qan = "123/4567/8"
            };

            var isValid = Validate(model, out var results);

            Assert.That(isValid, Is.True);
            Assert.That(results, Is.Empty);
        }

        [Test]
        public void Invalid_Qualification_Number_Fails()
        {
            var model = new TestModel
            {
                Qan = "INVALID"
            };

            var isValid = Validate(model, out var results);

            Assert.That(isValid, Is.False);
            Assert.That(results.Count, Is.GreaterThan(0));
        }

        [Test]
        public void Empty_Qualification_Number_Is_Valid()
        {
            var model = new TestModel
            {
                Qan = null
            };

            var isValid = Validate(model, out var results);

            Assert.That(isValid, Is.True);
        }

        [Test]
        public void Whitespace_Qualification_Number_Is_Valid()
        {
            var model = new TestModel
            {
                Qan = "   "
            };

            var isValid = Validate(model, out var results);

            Assert.That(isValid, Is.True);
        }
    }
}