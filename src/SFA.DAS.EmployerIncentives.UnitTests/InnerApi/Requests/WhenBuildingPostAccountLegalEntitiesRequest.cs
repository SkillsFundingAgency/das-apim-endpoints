using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;

namespace SFA.DAS.EmployerIncentives.UnitTests.InnerApi.Requests
{
    public class WhenBuildingPostAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_Post_Url_Is_Correctly_Built(long accountId, string baseUrl, List<string> data)
        {
            var actual = new PostAccountLegalEntityRequest(accountId){BaseUrl = baseUrl, Data = data};

            actual.PostUrl.Should().Be($"{baseUrl}accounts/{accountId}/legalentities");
            actual.Data.Should().BeEquivalentTo(data);
        }
    }
}