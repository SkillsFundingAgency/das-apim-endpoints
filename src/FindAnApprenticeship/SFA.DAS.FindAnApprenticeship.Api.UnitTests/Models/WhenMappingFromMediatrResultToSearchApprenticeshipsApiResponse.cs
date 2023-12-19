using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchApprenticeships;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingFromMediatrResultToSearchApprenticeshipsApiResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(SearchApprenticeshipsResult source)
        {
            var actual = (SearchApprenticeshipsApiResponse)source;

            actual.TotalApprenticeshipCount.Should().Be(source.TotalApprenticeshipCount);
            actual.Routes.Should().BeEquivalentTo(source.Routes);
            actual.Categories.Should().BeEquivalentTo(source.Categories);
            actual.Location.LocationName.Should().Be(source.LocationItem.Name);
            actual.Location.Lat.Should().Be(source.LocationItem.GeoPoint.First());
            actual.Location.Lon.Should().Be(source.LocationItem.GeoPoint.Last());
            actual.Vacancies.Should().BeEquivalentTo(source.Vacancies, options => options
                .Excluding(c => c.Address)
                .Excluding(c => c.AnonymousEmployerName)
                .Excluding(c => c.IsEmployerAnonymous)
                .Excluding(c => c.EmployerName)
                .Excluding(c => c.ApprenticeshipLevel));
                
            actual.Vacancies.FirstOrDefault().AddressLine1.Should().Be(source.Vacancies.FirstOrDefault().Address.AddressLine1);
            actual.Vacancies.FirstOrDefault().AddressLine2.Should().Be(source.Vacancies.FirstOrDefault().Address.AddressLine2);
            actual.Vacancies.FirstOrDefault().AddressLine3.Should().Be(source.Vacancies.FirstOrDefault().Address.AddressLine3);
            actual.Vacancies.FirstOrDefault().AddressLine4.Should().Be(source.Vacancies.FirstOrDefault().Address.AddressLine4);
            actual.Vacancies.FirstOrDefault().PostCode.Should().Be(source.Vacancies.FirstOrDefault().Address.Postcode);
            actual.Vacancies.FirstOrDefault().EmployerName.Should().Be(
                source.Vacancies.FirstOrDefault().IsEmployerAnonymous ? source.Vacancies.FirstOrDefault().AnonymousEmployerName :source.Vacancies.FirstOrDefault().EmployerName);
            actual.Vacancies.FirstOrDefault().ApprenticeshipLevel.Should().Be("1");
        }
        
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Null_Location(SearchApprenticeshipsResult source)
        {
            source.LocationItem = null;
            
            var actual = (SearchApprenticeshipsApiResponse)source;
            
            actual.Should().BeEquivalentTo(source, options => options.Excluding(c=>c.LocationItem)
                .Excluding(c =>c.Vacancies)
                .Excluding(c => c.PageSize)
                .Excluding(c => c.PageNumber)
                .Excluding(c => c.TotalPages)
            );
            actual.Location.Should().BeNull();
        }
    }
}