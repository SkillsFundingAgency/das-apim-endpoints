using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheCreateLogDataRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(LogCreate log)
        {
            var actual = new CreateLogDataRequest(log);

            actual.PostUrl.Should().Be("api/log/add");
        }
    }
}