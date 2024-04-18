using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests;

public class WhenBuildingGetQualificationReferenceTypesApiRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed()
    {
        var actual = new GetQualificationReferenceTypesApiRequest();

        actual.GetUrl.Should().Be("api/referencedata/qualifications");
    }
}