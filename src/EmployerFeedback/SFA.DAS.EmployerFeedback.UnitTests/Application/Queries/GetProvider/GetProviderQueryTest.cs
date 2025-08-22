using NUnit.Framework;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetProvider
{
    [TestFixture]
    public class GetProviderQueryTest
    {
        [Test]
        public void GetProviderQuery_ShouldHaveProviderIdProperty()
        {
            var query = new GetProviderQuery();
            query.ProviderId = 123;
            Assert.That(123, Is.EqualTo(query.ProviderId));
        }
    }
}