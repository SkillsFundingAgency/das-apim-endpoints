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
        public void Then_The_Post_Url_Is_Correctly_Built(ActivateCollectionCalendarPeriodRequestData data)
        {
            var actual = new ActivateCollectionCalendarPeriodRequest { Data = data };

            actual.PostUrl.Should().Be("collectionCalendar/period/activate");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}
