﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.InnerApi.Requests;

namespace SFA.DAS.Vacancies.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetEmployerAccountLegalEntityRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(string href)
        {
            var actual = new GetEmployerAccountLegalEntityRequest(href);

            actual.GetUrl.Should().Be($"{href}");
        }
    }
}