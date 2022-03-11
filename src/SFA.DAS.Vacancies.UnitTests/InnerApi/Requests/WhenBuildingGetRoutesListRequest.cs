using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.InnerApi.Requests;

namespace SFA.DAS.Vacancies.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetRoutesListRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Created()
        {
            //Arrange
            var actual = new GetRoutesListRequest();
            
            //Assert
            actual.GetUrl.Should().Be("api/courses/routes");
        }
    }
}