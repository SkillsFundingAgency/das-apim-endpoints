using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingGetVacanciesByReferenceRequest
{
    [Test, MoqAutoData]
    public void Then_The_Request_Url_Is_Correctly_Built(List<string> references)
    {
        var actual = new PostGetVacanciesByReferenceApiRequest(new PostGetVacanciesByReferenceApiRequestBody
        {
            VacancyReferences = references
        });

        actual.PostUrl.Should().Be($"/api/vacancies");
    }
}
