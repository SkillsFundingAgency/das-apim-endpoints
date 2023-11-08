using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.Commitments.GetRecentCommitment;
using SFA.DAS.ApprenticeAan.Application.InnerApi.StagedApprentices;
using SFA.DAS.ApprenticeAan.Application.MyApprenticeships.Commands.CreateMyApprenticeship;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.MyApprenticeships.Commands.CreateMyApprenticeship;

public class CreateMyApprenticeshipCommandTests
{
    [Test, AutoData]
    public void CreatesCommand_FromCommitment(GetRecentCommitmentQueryResult source)
    {
        source.Uln = "10000001";
        CreateMyApprenticeshipCommand sut = source;

        sut.Should()
            .BeEquivalentTo(source, options => options
                .WithAutoConversionFor(x => x.Path.Contains("Uln"))
                .ExcludingMissingMembers());
    }

    [Test, AutoData]
    public void CreatesCommand_FromStagedApprentice(GetStagedApprenticeResponse source)
    {
        CreateMyApprenticeshipCommand sut = source;

        sut.Should()
            .BeEquivalentTo(source, options => options.ExcludingMissingMembers());
    }
}
