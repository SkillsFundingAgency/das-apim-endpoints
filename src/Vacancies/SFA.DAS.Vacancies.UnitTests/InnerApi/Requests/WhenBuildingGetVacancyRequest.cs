using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.InnerApi.Requests;

namespace SFA.DAS.Vacancies.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetVacancyRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string vacancyReference)
        {
            //Act
            var actual = new GetVacancyRequest(vacancyReference);
            
            //Assert
            actual.GetUrl.Should().Be($"api/Vacancies/{vacancyReference}");
            actual.Version.Should().Be("2.0");
		}
    }
}