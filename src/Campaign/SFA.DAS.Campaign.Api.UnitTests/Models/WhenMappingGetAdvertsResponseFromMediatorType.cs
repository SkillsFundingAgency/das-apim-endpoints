using System.Collections.Generic;
using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Adverts;
using SFA.DAS.Campaign.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.Campaign.Api.UnitTests.Models
{
    public class WhenMappingGetAdvertsResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped_For_Non_Anon_Employer(GetAdvertsQueryResult source)
        {
            //Arrange
            foreach (var vacancy in source.Vacancies)
            {
                vacancy.IsEmployerAnonymous = false;
            }
            
            //Act
            var actual = (GetAdvertsResponse)source;

            //Assert
            actual.Routes.Should().BeEquivalentTo(source.Routes.Select(c=>new {Route = c.Name}).ToList());
            actual.TotalFound.Should().Be(source.TotalFound);
            actual.Location.Should().BeEquivalentTo(source.Location);
            actual.Vacancies.Should()
                .BeEquivalentTo(source.Vacancies, options => options
                    .Excluding(c => c.AnonymousEmployerName)
                    .Excluding(c => c.IsEmployerAnonymous)
                    .Excluding(c => c.StandardLarsCode)
                    );
        }

        [Test, AutoData]
        public void Then_If_The_Employer_Is_Anonymous_Then_Anon_Employer_Name_Used(GetAdvertsQueryResult source, string anonEmployerName)
        {
            //Arrange
            foreach (var vacancy in source.Vacancies)
            {
                vacancy.IsEmployerAnonymous = true;
                vacancy.AnonymousEmployerName = anonEmployerName;
            }
            
            //Act
            var actual = (GetAdvertsResponse)source;
            
            //Assert
            actual.Vacancies.ToList().TrueForAll(c => 
                c.EmployerName.Equals(anonEmployerName)
                ).Should().BeTrue();
            
        }

        [Test, AutoData]
        public void Then_If_No_Location_Response_Returned_Then_Mapped(List<GetRoutesListItem> routes)
        {
            //Arrange
            var source = new GetAdvertsQueryResult
            {
                Location = null,
                Routes = routes,
                Vacancies = new List<GetVacanciesListItem>(),
                TotalFound = 0
            };

            //Act
            var actual = (GetAdvertsResponse)source;
            
            //Assert
            actual.TotalFound.Should().Be(0);
            actual.Routes.Should().BeEquivalentTo(routes.Select(c=>new {Route = c.Name}).ToList());
            actual.Vacancies.Should().BeEmpty();
            actual.Location.Should().BeNull();
        }
    }
}