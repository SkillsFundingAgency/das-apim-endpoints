using AutoFixture.NUnit3;
using SFA.DAS.EmployerAan.Application.InnerApi.Standards.Requests;

namespace SFA.DAS.EmployerAan.Application.UnitTests.MyApprenticeships.Queries.GetMyApprenticeships;

public class GetFrameworkQueryRequestTests
{
    [Test]
    [AutoData]
    public void Constructor_BuildsRequest(string trainingCode)
    {
        var request = new GetFrameworkQueryRequest(trainingCode);

        Assert.Multiple(() =>
        {
            Assert.That(request.TrainingCode, Is.EqualTo(trainingCode));
            Assert.That(request.GetUrl, Is.EqualTo($"api/courses/Frameworks/{trainingCode}"));
        });
    }
}