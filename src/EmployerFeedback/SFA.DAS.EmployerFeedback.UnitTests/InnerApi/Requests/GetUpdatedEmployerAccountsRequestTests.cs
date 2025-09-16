using NUnit.Framework;
using SFA.DAS.EmployerFeedback.InnerApi.Requests;
using System;

namespace SFA.DAS.EmployerFeedback.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class GetUpdatedEmployerAccountsRequestTests
    {
        [Test]
        public void Constructor_SetsProperties_And_Url_With_SinceDate()
        {
            var since = DateTime.UtcNow;
            var request = new GetUpdatedEmployerAccountsRequest(since, 2, 50);
            Assert.That(request.SinceDate, Is.EqualTo(since));
            Assert.That(request.PageNumber, Is.EqualTo(2));
            Assert.That(request.PageSize, Is.EqualTo(50));
            Assert.That(request.GetUrl, Does.Contain($"SinceDate={since:O}"));
            Assert.That(request.GetUrl, Does.Contain("page=2"));
            Assert.That(request.GetUrl, Does.Contain("pageSize=50"));
        }

        [Test]
        public void Constructor_SetsProperties_And_Url_Without_SinceDate()
        {
            var request = new GetUpdatedEmployerAccountsRequest(null, 1, 10);
            Assert.That(request.SinceDate, Is.Null);
            Assert.That(request.PageNumber, Is.EqualTo(1));
            Assert.That(request.PageSize, Is.EqualTo(10));
            Assert.That(request.GetUrl, Is.EqualTo("api/accounts/update?page=1&pageSize=10"));
        }
    }
}
