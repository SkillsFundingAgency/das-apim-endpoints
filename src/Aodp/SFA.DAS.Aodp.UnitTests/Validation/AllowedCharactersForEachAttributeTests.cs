using NUnit.Framework;
using SFA.DAS.Aodp.UnitTests.Validation;
using SFA.DAS.Aodp.Validation;
using SFA.DAS.Aodp.Validation.Attributes;
using System.Collections.Generic;

namespace SFA.DAS.Aodp.Api.UnitTests.Validation.Attributes
{
    [TestFixture]
    public class AllowedCharactersForEachAttributeTests
    {
        private class Model
        {
            [AllowedCharactersForEach(TextCharacterProfile.FreeText)]
            public List<string>? Values { get; set; }
        }

        [Test]
        public void All_valid_items_pass()
        {
            var model = new Model
            {
                Values = new List<string> { "A", "B", "C" }
            };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.True);
            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void One_invalid_item_fails()
        {
            var model = new Model
            {
                Values = new List<string> { "A", "<script>" }
            };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Null_list_passes()
        {
            var model = new Model
            {
                Values = null
            };

            var result = ValidationTestHelper.Validate(model, out var errors);

            Assert.That(result, Is.True);
        }
    }
}