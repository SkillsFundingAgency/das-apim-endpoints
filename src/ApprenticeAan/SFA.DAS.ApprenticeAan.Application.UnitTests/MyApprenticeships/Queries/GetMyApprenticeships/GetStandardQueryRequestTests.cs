using AutoFixture.NUnit3;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Standards.Requests;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeships.Queries.GetMyApprenticeships;

public class GetStandardQueryRequestTests
{
    [Test]
    [AutoData]
    public void Constructor_BuildsRequest(string standardUid)
    {
        var request = new GetStandardRequest(standardUid);

        Assert.Multiple(() =>
        {
            Assert.That(request.StandardUid, Is.EqualTo(standardUid));
            Assert.That(request.GetUrl, Is.EqualTo($"api/courses/Standards/{standardUid}"));
        });
    }
}