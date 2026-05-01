using SFA.DAS.Approvals.Api.Models;
using SFA.DAS.Approvals.InnerApi.Responses;

namespace SFA.DAS.Approvals.Api.UnitTests.Models
{
    public class WhenMappingGetStandardResponseFromMediatorResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetStandardsListItem source)
        {
            //Act
            var actual = (GetStandardResponse)source;

            //Assert
            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.CourseDates)
                .Excluding(c => c.TypicalDuration)
                .Excluding(c => c.IsActive)
                .Excluding(c => c.StandardUId)
                .Excluding(c => c.LarsCode)
                .ExcludingMissingMembers()
            );
            actual.EffectiveFrom.Should().Be(source.CourseDates.EffectiveFrom);
            actual.EffectiveTo.Should().Be(source.CourseDates.EffectiveTo);
            actual.LastDateForNewStarts.Should().Be(source.CourseDates.LastDateStarts);
            actual.Duration.Should().Be(source.TypicalDuration);
            actual.LarsCode.Should().Be(source.LarsCode);
            actual.IfateReferenceNumber.Should().Be(source.IfateReferenceNumber);
            actual.Version.Should().Be(source.Version);
            actual.VersionMajor.Should().Be(source.VersionMajor);
            actual.VersionMinor.Should().Be(source.VersionMinor);
            actual.VersionDetail.Should().BeEquivalentTo(source.VersionDetail);
            actual.Route.Should().BeEquivalentTo(source.Route);
            actual.ApprenticeshipType.Should().Be(source.ApprenticeshipType);
        }
    }
}