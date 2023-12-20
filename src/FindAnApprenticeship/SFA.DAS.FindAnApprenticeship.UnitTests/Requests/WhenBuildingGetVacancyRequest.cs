using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingGetVacancyRequest
{
    [Test, AutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(string vacancyReference)
    {
        var actual = new GetVacancyRequest(vacancyReference);

        actual.GetUrl.Should().Be($"/api/vacancies/{vacancyReference}");
    }
}
