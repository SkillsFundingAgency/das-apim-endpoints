using NUnit.Framework;

namespace SFA.DAS.EmployerFeedback.Application.Queries.GetProvider
{
    [TestFixture]
    public class GetProviderQueryTest
    {
        [Test]
        public void GetProviderQuery_ShouldHaveProviderIdProperty()
        {
            // Arrange
            var query = new GetProviderQuery();
            // Act
            query.ProviderId = 123;
            // Assert
            Assert.That(123, Is.EqualTo(query.ProviderId));
        }
    }
}