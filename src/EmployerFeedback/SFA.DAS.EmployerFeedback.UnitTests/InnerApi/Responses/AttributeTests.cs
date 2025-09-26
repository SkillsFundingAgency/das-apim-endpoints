using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Responses
{
    [TestFixture]
    public class AttributeTests
    {
        [Test]
        public void CanSetAndGetProperties()
        {
            var attribute = new GetAttributesResponse
            {
                AttributeId = 123,
                AttributeName = "Test Attribute"
            };
            Assert.That(attribute.AttributeId, Is.EqualTo(123));
            Assert.That(attribute.AttributeName, Is.EqualTo("Test Attribute"));
        }

        [Test]
        public void DefaultValues_AreSet()
        {
            var attribute = new GetAttributesResponse();
            Assert.That(attribute.AttributeId, Is.EqualTo(0));
            Assert.That(attribute.AttributeName, Is.Null);
        }
    }
}
