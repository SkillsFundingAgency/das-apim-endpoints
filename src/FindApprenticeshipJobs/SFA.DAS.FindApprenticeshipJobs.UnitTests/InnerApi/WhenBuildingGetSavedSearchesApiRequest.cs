using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi
{
    public class WhenBuildingGetSavedSearchesApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            string lastRunDateTime,
            int pageNumber,
            int pageSize)
        {
            var actual = new GetSavedSearchesApiRequest(lastRunDateTime, pageNumber, pageSize);

            actual.GetUrl.Should().Be($"api/savedSearches?lastRunDateFilter={lastRunDateTime}&pageNumber={pageNumber}&pageSize={pageSize}");
        }
    }
}