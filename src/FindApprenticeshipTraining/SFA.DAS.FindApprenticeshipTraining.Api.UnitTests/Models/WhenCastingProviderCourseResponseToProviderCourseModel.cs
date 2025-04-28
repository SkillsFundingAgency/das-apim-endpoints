using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models;

public sealed class WhenCastingProviderCourseResponseToProviderCourseModel
{
    [Test]
    [MoqAutoData]
    public void Then_All_Properties_Must_Map_Correctly(ProviderCourseResponse response)
    {
        ProviderCourseModel sut = response;

        Assert.Multiple(() =>
        {
            Assert.That(sut.CourseName, Is.EqualTo(response.CourseName));
            Assert.That(sut.LarsCode, Is.EqualTo(response.LarsCode));
            Assert.That(sut.Level, Is.EqualTo(response.Level));
            Assert.That(sut.IfateReferenceNumber, Is.EqualTo(response.IfateReferenceNumber));
        });
    }
}
