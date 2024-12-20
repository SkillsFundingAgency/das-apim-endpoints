using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi
{
    [TestFixture]
    public class WhenBuildingDeleteSavedSearchRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(Guid id)
        {
            var actual = new DeleteSavedSearchRequest(id);

            actual.DeleteUrl.Should().Be($"api/SavedSearches/{id}");
        }
    }
}
