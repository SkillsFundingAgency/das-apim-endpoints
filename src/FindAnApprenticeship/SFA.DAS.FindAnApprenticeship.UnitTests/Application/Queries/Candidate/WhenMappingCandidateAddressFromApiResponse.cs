using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.Candidate;

public class WhenMappingCandidateAddressFromApiResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetAddressApiResponse source)
    {
        var actual = (GetApplicationQueryResult.CandidateAddress)source;

        actual.Should().BeEquivalentTo(source, options => options.Excluding(fil => fil.CandidateId));
    }
}