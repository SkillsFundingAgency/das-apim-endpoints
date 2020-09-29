using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPatchSignAgreementRequest
    {
        [Test, AutoData]
        public void Then_The_Patch_Url_Is_Correctly_Built(long accountId, long accountLegalEntityId, SignAgreementRequest data)
        {
            var actual = new PatchSignAgreementRequest(accountId,accountLegalEntityId){Data = data};

            actual.PatchUrl.Should().Be($"accounts/{accountId}/legalentities/{accountLegalEntityId}");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}