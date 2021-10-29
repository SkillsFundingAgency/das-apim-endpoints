﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.UnitTests.InnerApi.Requests
{

    public class WhenbuildingGetVancanciesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Build(int pageNumber, int pageSize)
        {
            var actual = new GetVacanciesRequest();

            actual.GetUrl.Should().Be($"api/Vacancies?{pageNumber}&{pageSize}");
        }
    }
}