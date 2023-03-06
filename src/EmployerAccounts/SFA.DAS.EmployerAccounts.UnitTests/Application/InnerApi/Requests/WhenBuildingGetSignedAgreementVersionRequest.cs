using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;

namespace SFA.DAS.EmployerAccounts.UnitTests.Application.InnerApi.Requests
{
    public class WhenBuildingGetSignedAgreementVersionRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(string hashedAccountId)
        {
            var actual = new GetSignedAgreementVersionRequest(hashedAccountId);
            
            actual.GetUrl.Should().Be($"api/accounts/{hashedAccountId}/signed-agreement-version");
        }
    }
}