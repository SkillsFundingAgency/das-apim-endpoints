using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingGetUnmetEmployerDemandsRequest
    {
        [Test, AutoData]
        public void Then_Creates_Url_Correctly(uint ageOfDemandInDays)
        {
            //Arrange
            var actual = new GetUnmetEmployerDemandsRequest(ageOfDemandInDays);
            
            //Assert
            actual.GetUrl.Should().Be($"api/Demand/unmet?ageOfDemandInDays={ageOfDemandInDays}");
        }
    }
}