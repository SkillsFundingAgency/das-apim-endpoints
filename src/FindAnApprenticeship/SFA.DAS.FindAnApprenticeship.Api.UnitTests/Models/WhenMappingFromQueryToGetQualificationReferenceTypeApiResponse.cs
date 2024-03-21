using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetAddQualification;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromQueryToGetQualificationReferenceTypeApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetAddQualificationQueryResult source)
    {
        var actual = (GetQualificationReferenceTypeApiResponse)source;

        actual.QualificationType.Should().BeEquivalentTo(source.QualificationType);
    }
}