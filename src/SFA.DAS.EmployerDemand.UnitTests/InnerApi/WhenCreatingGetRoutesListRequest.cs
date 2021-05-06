using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetRoutesListRequest
    {
        [Test]
        public void Then_Sets_Url_Correctly()
        {
            var request = new GetRoutesListRequest();

            request.GetUrl.Should().Be("api/courses/routes");
        }
    }
}