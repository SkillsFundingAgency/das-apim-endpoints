﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.EducationalOrganisations;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingIdentifiableOrganisationTypesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build()
        {
            var actual = new IdentifiableOrganisationTypesRequest();

            var expected = "api/EducationalOrganisations/IdentifiableOrganisationTypes";

            actual.GetUrl.Should().Be(expected);
        }
    }
}
