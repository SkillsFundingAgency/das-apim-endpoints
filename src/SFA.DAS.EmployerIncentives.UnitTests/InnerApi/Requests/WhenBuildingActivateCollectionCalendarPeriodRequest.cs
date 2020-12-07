using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CollectionCalendar;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingActivateCollectionCalendarPeriodRequest
    {
        [Test, AutoData]
        public void Then_The_Patch_Url_Is_Correctly_Built(ActivateCollectionCalendarPeriodRequestData data)
        {
            var actual = new ActivateCollectionCalendarPeriodRequest { Data = data };

            actual.PatchUrl.Should().Be("collectionCalendar/period/active");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}
