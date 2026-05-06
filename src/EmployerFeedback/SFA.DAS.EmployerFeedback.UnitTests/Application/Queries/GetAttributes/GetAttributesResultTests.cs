using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAttributes;
using SFA.DAS.EmployerFeedback.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetAttributes
{
    [TestFixture]
    public class GetAttributesResultTests
    {
        [Test]
        public void CanSetAndGetAttributes()
        {
            var attributes = new List<GetAttributesResponse>
            {
                new GetAttributesResponse { AttributeId = 1, AttributeName = "Test1" },
                new GetAttributesResponse { AttributeId = 2, AttributeName = "Test2" }
            };
            var result = new GetAttributesResult { Attributes = attributes };
            Assert.That(result.Attributes, Is.EqualTo(attributes));
        }

        [Test]
        public void Attributes_DefaultsToNull()
        {
            var result = new GetAttributesResult();
            Assert.That(result.Attributes, Is.Null);
        }
    }
}
