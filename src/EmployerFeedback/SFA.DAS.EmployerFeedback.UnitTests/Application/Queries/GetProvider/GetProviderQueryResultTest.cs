using NUnit.Framework;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetProvider
{
    [TestFixture]
    public class GetProviderQueryResultTest
    {
        [Test]
        public void GetProviderQueryResult_ShouldHaveProperties()
        {
            var result = new GetProviderQueryResult
            {
                Name = "Test Provider",
                ProviderId = 12345678
            };
            Assert.That("Test Provider", Is.EqualTo(result.Name));
            Assert.That(12345678, Is.EqualTo(result.ProviderId));
        }
    }
}