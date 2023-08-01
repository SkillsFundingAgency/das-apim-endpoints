using AutoFixture.NUnit3;
using SFA.DAS.ApprenticeAan.Application.InnerApi.MyApprenticeships;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeships.Queries.GetMyApprenticeships;

public class GetMyApprenticeshipRequestTests
{
    [Test]
    [AutoData]
    public void Constructor_BuildsRequest(Guid id)
    {
        var request = new GetMyApprenticeshipRequest(id);

        Assert.Multiple(() =>
        {
            Assert.That(request.Id, Is.EqualTo(id));
            Assert.That(request.GetUrl, Is.EqualTo($"apprentices/{id}/MyApprenticeship"));
        });
    }
}