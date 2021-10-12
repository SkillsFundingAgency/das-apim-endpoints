﻿using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.InnerApi.Requests;

namespace SFA.DAS.Vacancies.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetAllEmployerAccountLegalEntitiesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string encodedAccountId)
        {
            var actual = new GetAllEmployerAccountLegalEntitiesRequest(encodedAccountId);

            actual.GetAllUrl.Should().Be($"api/accounts/{encodedAccountId}/legalentities");
        }
    }
}