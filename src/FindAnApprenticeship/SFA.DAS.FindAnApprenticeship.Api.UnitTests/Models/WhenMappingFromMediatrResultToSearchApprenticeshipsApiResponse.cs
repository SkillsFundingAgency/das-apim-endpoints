using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingFromMediatrResultToSearchApprenticeshipsApiResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(SearchApprenticeshipsResult source)
        {
            var actual = (SearchApprenticeshipsApiResponse)source;

            actual.TotalApprenticeshipCount.Should().Be(source.TotalApprenticeshipCount);
            actual.TotalCompetitiveVacanciesCount.Should().Be(source.TotalWageTypeVacanciesCount);
            actual.TotalFound.Should().Be(source.TotalFound);
            actual.Routes.Should().BeEquivalentTo(source.Routes);
            actual.Levels.Should().BeEquivalentTo(source.Levels, options => options.Excluding(c => c.Code));
            actual.Location.LocationName.Should().Be(source.LocationItem.Name);
            actual.Location.Lat.Should().Be(source.LocationItem.GeoPoint.First());
            actual.Location.Lon.Should().Be(source.LocationItem.GeoPoint.Last());
            actual.VacancyReference.Should().Be(source.VacancyReference);
            actual.Vacancies.Should().BeEquivalentTo(source.Vacancies, options => options
                .Excluding(c => c.Address)
                .Excluding(c => c.AnonymousEmployerName)
                .Excluding(c => c.IsEmployerAnonymous)
                .Excluding(c => c.EmployerName)
                .Excluding(c => c.ApprenticeshipLevel)
                .Excluding(c => c.Location)
                .Excluding(c => c.EmploymentLocationOption) //TBC : should be removed in the later stages
            );
                
            actual.Vacancies.FirstOrDefault().AddressLine1.Should().Be(source.Vacancies.FirstOrDefault().Address.AddressLine1);
            actual.Vacancies.FirstOrDefault().AddressLine2.Should().Be(source.Vacancies.FirstOrDefault().Address.AddressLine2);
            actual.Vacancies.FirstOrDefault().AddressLine3.Should().Be(source.Vacancies.FirstOrDefault().Address.AddressLine3);
            actual.Vacancies.FirstOrDefault().AddressLine4.Should().Be(source.Vacancies.FirstOrDefault().Address.AddressLine4);
            actual.Vacancies.FirstOrDefault().PostCode.Should().Be(source.Vacancies.FirstOrDefault().Address.Postcode);
            actual.Vacancies.FirstOrDefault().EmployerName.Should().Be(
                source.Vacancies.FirstOrDefault().IsEmployerAnonymous ? source.Vacancies.FirstOrDefault().AnonymousEmployerName :source.Vacancies.FirstOrDefault().EmployerName);
            actual.Vacancies.FirstOrDefault().ApprenticeshipLevel.Should().Be(source.Vacancies.FirstOrDefault().ApprenticeshipLevel);
            actual.Vacancies.FirstOrDefault().Lat.Should().Be(source.Vacancies.FirstOrDefault().Location.Lat);
            actual.Vacancies.FirstOrDefault().Lon.Should().Be(source.Vacancies.FirstOrDefault().Location.Lon);
            actual.Vacancies.FirstOrDefault().IsPrimaryLocation.Should().Be(source.Vacancies.FirstOrDefault().IsPrimaryLocation);
            actual.Vacancies.FirstOrDefault().EmploymentLocationInformation.Should().Be(source.Vacancies.FirstOrDefault().EmploymentLocationInformation);
            //actual.Vacancies.FirstOrDefault().EmploymentLocationOption.Should().Be(source.Vacancies.FirstOrDefault().EmploymentLocationOption); TBC
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Null_Location(SearchApprenticeshipsResult source)
        {
            source.LocationItem = null;
            
            var actual = (SearchApprenticeshipsApiResponse)source;
            
            actual.Should().BeEquivalentTo(source, options => options.Excluding(c=>c.LocationItem)
                .Excluding(c =>c.Vacancies)
                .Excluding(c => c.Levels)
                .Excluding(c => c.PageSize)
                .Excluding(c => c.PageNumber)
                .Excluding(c => c.TotalPages)
                .Excluding(c => c.TotalWageTypeVacanciesCount)
            );
            actual.Location.Should().BeNull();
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_GetVacanciesListResponse(GetVacanciesListItem source)
        {
            var actual = (GetVacanciesListResponseItem)source;

            actual.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.Application)
                .Excluding(c => c.EmployerName)
                .Excluding(c => c.Address)
                .Excluding(c => c.AnonymousEmployerName)
                .Excluding(c => c.IsEmployerAnonymous)
                .Excluding(c => c.Location)
                .Excluding(c => c.EmploymentLocationOption) //TBC : should be removed in the later stages
            );
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_CandidateApplicationDetails(GetVacanciesListItem.CandidateApplication source)
        {
            var actual = (CandidateApplicationDetails)source;

            actual.Should().BeEquivalentTo(source);
        }

        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_CandidateApplicationDetails_When_Source_Is_Null()
        {
            var actual = (CandidateApplicationDetails)(GetVacanciesListItem.CandidateApplication) null;

            actual.Should().BeNull();
        }
    }
}