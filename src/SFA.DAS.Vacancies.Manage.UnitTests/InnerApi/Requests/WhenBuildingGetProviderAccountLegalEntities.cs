﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetProviderAccountLegalEntities
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(int ukprn)
        {
            var actual = new GetProviderAccountLegalEntitiesRequest(ukprn);

            actual.GetUrl.Should().Be($"accountproviderlegalentities?ukprn={ukprn}&operations=1&operations=2");
        }
    }
}