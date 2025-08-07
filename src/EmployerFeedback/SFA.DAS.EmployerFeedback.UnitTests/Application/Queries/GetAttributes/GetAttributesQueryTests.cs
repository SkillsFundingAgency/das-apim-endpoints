using NUnit.Framework;
using SFA.DAS.EmployerFeedback.Application.Queries.GetAttributes;

namespace SFA.DAS.EmployerFeedback.UnitTests.Application.Queries.GetAttributes
{
    [TestFixture]
    public class GetAttributesQueryTests
    {
        [Test]
        public void Constructor_CreatesInstance()
        {
            var query = new GetAttributesQuery();
            Assert.That(query, Is.Not.Null);
        }
    }
}
