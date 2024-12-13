using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi
{
    public class WhenBuildingPatchSavedSearchApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            Guid id)
        {
            var actual = new PatchSavedSearchApiRequest(id, null);

            actual.PatchUrl.Should().Be($"api/savedSearches/{id}");
        }
    }
}