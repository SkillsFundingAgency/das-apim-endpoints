using SFA.DAS.Aodp.UnitTests.Validation;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Api.UnitTests.Validation.Attributes
{
    [TestFixture]
    public class MessageTypeAttributeTests
    {
        private class Model
        {
            [MessageType]
            public string? Value { get; set; }
        }

        [Test]
        public void Valid_message_type_passes()
        {
            var model = new Model { Value = "UnlockApplication" };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Invalid_message_type_fails()
        {
            var model = new Model { Value = "InvalidValue" };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Null_value_fails()
        {
            var model = new Model { Value = null };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.False);
        }
    }
}