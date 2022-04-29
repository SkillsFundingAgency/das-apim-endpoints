using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using SFA.DAS.Campaign.Application.Queries.Adverts;
using SFA.DAS.Campaign.Configuration;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Application.Queries.Adverts
{
    public class WhenHandlingGetAdvertsQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Routes_Are_Filtered_And_Postcode_Searched_And_Adverts_Returned(
            string findAnApprenticeshipBaseUrl,
            GetAdvertsQuery query,
            List<string> categories,
            LocationItem locationItem,
            GetVacanciesResponse vacanciesResponse,
            GetRoutesListResponse getRoutesListResponse,
            GetStandardsListResponse standards,
            [Frozen] Mock<IOptions<CampaignConfiguration>> configuration,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> faaApiClient,
            GetAdvertsQueryHandler handler)
        {
            //Arrange
            query.Route = standards.Standards.First().Route.ToLower();
            configuration.Object.Value.FindAnApprenticeshipBaseUrl = findAnApprenticeshipBaseUrl;
            var expectedAdvertUrl = new GetVacanciesRequest(0, 20, null, null, null, new List<int>{ standards.Standards.First().LarsCode}, null,
                locationItem.GeoPoint.First(), locationItem.GeoPoint.Last(), query.Distance, null,
                "DistanceAsc");
            locationLookupService.Setup(x => x.GetLocationInformation(query.Postcode, 0, 0, false))
                .ReturnsAsync(locationItem);
            faaApiClient
                .Setup(x => x.Get<GetVacanciesResponse>(It.Is<GetVacanciesRequest>(c =>
                    c.GetUrl.Equals(expectedAdvertUrl.GetUrl)))).ReturnsAsync(vacanciesResponse);
            courseService.Setup(x => x.GetRoutes()).ReturnsAsync(getRoutesListResponse);
            courseService.Setup(x => x.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse))).ReturnsAsync(standards);

            var actual = await handler.Handle(query, CancellationToken.None);

            actual.Vacancies.Should().BeEquivalentTo(vacanciesResponse.ApprenticeshipVacancies);
            actual.TotalFound.Should().Be(vacanciesResponse.TotalFound);
            actual.Routes.Should().BeEquivalentTo(getRoutesListResponse.Routes);
            actual.Location.Should().BeEquivalentTo(locationItem);
            
            foreach (var vacancy in actual.Vacancies)
            {
                vacancy.VacancyUrl.Should().Be($"{findAnApprenticeshipBaseUrl}/apprenticeship/reference/{vacancy.VacancyReference}");
            }
        }

        [Test, MoqAutoData]
        public async Task Then_If_The_Postcode_Is_Not_Valid_Then_No_Results_Returned(
            GetAdvertsQuery query,
            GetRoutesListResponse getRoutesListResponse,
            [Frozen] Mock<ICourseService> courseService,
            [Frozen] Mock<ILocationLookupService> locationLookupService,
            GetAdvertsQueryHandler handler)
        {
            locationLookupService.Setup(x => x.GetLocationInformation(query.Postcode, 0, 0, false))
                .ReturnsAsync((LocationItem)null);
            courseService.Setup(x => x.GetRoutes()).ReturnsAsync(getRoutesListResponse);
            
            var actual = await handler.Handle(query, CancellationToken.None);
            
            actual.Routes.Should().BeEquivalentTo(getRoutesListResponse.Routes);
            actual.TotalFound.Should().Be(0);
            actual.Location.Should().BeNull();
            actual.Vacancies.Should().BeEmpty();
        }
    }
}