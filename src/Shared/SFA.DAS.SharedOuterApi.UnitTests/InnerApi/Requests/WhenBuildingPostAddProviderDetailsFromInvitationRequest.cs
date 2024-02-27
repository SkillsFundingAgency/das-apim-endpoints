using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostAddProviderDetailsFromInvitationRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(long accountId, long ukprn, string correlationId,
            string userRef, string email, string firstName, string lastName)
        {
            var actual = new PostAddProviderDetailsFromInvitationRequest(accountId, ukprn, correlationId,
                userRef, email, firstName, lastName);

            actual.PostUrl.Should().Be($"accounts/{accountId}/providers/invitation");
        }       
    }
}
