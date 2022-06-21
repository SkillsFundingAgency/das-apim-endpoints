using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetProvidersRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Constructed()
        {
            //Act
            var actual = new GetProvidersRequest();
            
            //Assert
            actual.GetUrl.Should().Be("api/providers/");
        }
    }
}