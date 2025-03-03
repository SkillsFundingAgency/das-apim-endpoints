using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.CandidateApi.Requests
{
    [TestFixture]
    public class WhenBuildingDeleteAccountApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid candidateId)
        {
            var actual = new DeleteAccountApiRequest(candidateId);

            actual.DeleteUrl.Should().Be($"api/candidates/{candidateId}");
        }
    }
}