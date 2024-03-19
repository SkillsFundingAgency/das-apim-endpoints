using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetQualificationTypes;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingFromQueryToGetQualificationReferenceTypesApiResponse
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped(GetQualificationTypesQueryResult source)
    {
        var actual = (GetQualificationReferenceTypesApiResponse)source;

        actual.QualificationTypes.Should().BeEquivalentTo(source.QualificationTypes);
    }
}