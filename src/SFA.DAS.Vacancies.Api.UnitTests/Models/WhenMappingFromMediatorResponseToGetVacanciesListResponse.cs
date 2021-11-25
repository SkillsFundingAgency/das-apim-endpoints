using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.Vacancies.Queries;
using FluentAssertions;
using SFA.DAS.Vacancies.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Api.UnitTests.Models
{
    class WhenMappingFromMediatorResponseToGetVacanciesListResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_are_mapped(GetVacanciesQueryResult source)
        {
            var actual = (GetVacanciesListResponse) source ;

            actual.Should().BeEquivalentTo(source);

        }
    }
}
