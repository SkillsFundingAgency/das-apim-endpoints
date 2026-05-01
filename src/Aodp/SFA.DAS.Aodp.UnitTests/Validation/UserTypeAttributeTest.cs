using SFA.DAS.Aodp.UnitTests.Validation;
using SFA.DAS.Aodp.Validation;

namespace SFA.DAS.Aodp.Api.UnitTests.Validation.Attributes
{
    [TestFixture]
    public class UserTypeAttributeTests
    {
        private class Model
        {
            [UserType]
            public string? Value { get; set; }
        }

        [Test]
        public void Valid_user_type_passes()
        {
            var model = new Model { Value = "SkillsEngland" };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Invalid_user_type_fails()
        {
            var model = new Model { Value = "RandomUser" };

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