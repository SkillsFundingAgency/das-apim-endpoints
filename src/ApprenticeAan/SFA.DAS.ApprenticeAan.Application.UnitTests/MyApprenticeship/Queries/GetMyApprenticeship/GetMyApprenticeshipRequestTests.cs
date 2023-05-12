using AutoFixture.NUnit3;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeship.Queries.GetMyApprenticeship;

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