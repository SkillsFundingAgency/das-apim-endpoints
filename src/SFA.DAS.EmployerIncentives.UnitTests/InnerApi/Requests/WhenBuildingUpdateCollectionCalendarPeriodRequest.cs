using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingUpdateCollectionCalendarPeriodRequest
    {
        [Test, AutoData]
        public void Then_The_Patch_Url_Is_Correctly_Built(UpdateCollectionCalendarPeriodRequestData data)
        {
            var actual = new UpdateCollectionCalendarPeriodRequest { Data = data };

            actual.PatchUrl.Should().Be("collectionPeriods");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}
