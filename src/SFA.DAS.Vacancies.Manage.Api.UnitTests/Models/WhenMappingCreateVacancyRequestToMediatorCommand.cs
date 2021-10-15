using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;

namespace SFA.DAS.Vacancies.Manage.Api.UnitTests.Models
{
    public class WhenMappingCreateVacancyRequestToMediatorCommand
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(CreateVacancyRequest source)
        {
            var actual = (PostVacancyRequestData) source;
            
            source.Should().BeEquivalentTo(actual);
        }
    }
}