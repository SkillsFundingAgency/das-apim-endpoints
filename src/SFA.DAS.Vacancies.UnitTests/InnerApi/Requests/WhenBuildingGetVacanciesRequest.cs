using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.Models;

namespace SFA.DAS.Vacancies.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetVacanciesRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(
            int pageNumber, 
            int pageSize,
            string accountPublicHashedId,
            string accountLegalEntityPublicHashedId,
            int? ukprn,
            int? standardLarsCode,
            bool? nationwideOnly,
            double? lat,
            double? lon,
            uint? distanceInMiles,
            string route,
            uint? postedInLastNumberOfDays,
            VacancySort? sort)
        {
            var actual = new GetVacanciesRequest(pageNumber, pageSize, accountLegalEntityPublicHashedId, ukprn, accountPublicHashedId, standardLarsCode, nationwideOnly, lat, lon, distanceInMiles, route, postedInLastNumberOfDays, sort);

            actual.GetUrl.Should().Be($"api/Vacancies?pageNumber={pageNumber}&pageSize={pageSize}&ukprn={ukprn}&accountLegalEntityPublicHashedId={accountLegalEntityPublicHashedId}&accountPublicHashedId={accountPublicHashedId}&standardLarsCode={standardLarsCode}&nationwideOnly={nationwideOnly}&lat={lat}&lon={lon}&distanceInMiles={distanceInMiles}&route={route}&sort={sort}&postedInLastNumberOfDays={postedInLastNumberOfDays}");
        }

        [Test, AutoData]
        public void Then_Is_Correctly_Built_With_No_Optionals(int pageNumber, 
            int pageSize)
        {
            var actual = new GetVacanciesRequest(pageNumber, pageSize);

            actual.GetUrl.Should().Be($"api/Vacancies?pageNumber={pageNumber}&pageSize={pageSize}&ukprn=&accountLegalEntityPublicHashedId=&accountPublicHashedId=&standardLarsCode=&nationwideOnly=&lat=&lon=&distanceInMiles=&route=&sort=&postedInLastNumberOfDays=");
        }
    }
}