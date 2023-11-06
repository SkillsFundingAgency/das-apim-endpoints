using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EarlyConnect.InnerApi.Requests;

namespace SFA.DAS.EarlyConnect.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheCreateLogRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(CreateLog log)
        {
            var actual = new CreateLogRequest(log);

            actual.PostUrl.Should().Be("api/log/add");
        }
    }
}