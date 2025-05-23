﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Domain.Models;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.InnerApi
{
    public class WhenBuildingGetVacanciesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Url_Is_Correctly_Built(
            double lat,
            double lon,
            int distance,
            VacancySort sort,
            string whatSearchTerm,
            int pageNumber,
            int pageSize,
            List<string> categories,
            List<int>? levels,
            bool disabilityConfident,
            bool excludeNational)
        {
            var actual = new GetVacanciesRequest(lat, lon, distance, whatSearchTerm, pageNumber, pageSize, categories, levels, sort, disabilityConfident, excludeNational, new List<VacancyDataSource>
                {
                    VacancyDataSource.Nhs
                });

            actual.GetUrl
                .Should()
                .Be($"/api/vacancies?lat={lat}&lon={lon}&distanceInMiles={distance}&sort={sort}&pageNumber={pageNumber}&pageSize={pageSize}&categories={string.Join("&categories=", categories)}&levels={string.Join("&levels=", levels)}&searchTerm={whatSearchTerm}&disabilityConfident={disabilityConfident}&excludeNational={excludeNational}&additionalDataSources=Nhs&postedInLastNumberOfDays=7");
            actual.Version.Should().Be("2.0");
        }
    }
}
