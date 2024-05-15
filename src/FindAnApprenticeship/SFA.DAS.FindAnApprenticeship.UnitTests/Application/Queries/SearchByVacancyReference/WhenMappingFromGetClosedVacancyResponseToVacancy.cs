using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SearchByVacancyReference;

public class WhenMappingFromGetClosedVacancyResponseToVacancy
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped_For_Closed_Vacancy(GetClosedVacancyResponse source)
    {
        var actual = (GetApprenticeshipVacancyQueryResult.Vacancy)source;
        
        actual.ClosingDate.Should().Be(source.ClosedDate);
        actual.Id.Should().Be(source.VacancyReferenceNumeric.ToString());
        actual.VacancyReference.Should().Be(source.VacancyReference);
        actual.EmployerName.Should().Be(source.EmployerName);
        actual.Title.Should().Be(source.Title);
        actual.IsClosed.Should().BeTrue();
        actual.Address.AddressLine1.Should().Be(source.EmployerLocation?.AddressLine1);
        actual.Address.AddressLine2.Should().Be(source.EmployerLocation?.AddressLine2);
        actual.Address.AddressLine3.Should().Be(source.EmployerLocation?.AddressLine3);
        actual.Address.AddressLine4.Should().Be(source.EmployerLocation?.AddressLine4);
        actual.Address.Postcode.Should().Be(source.EmployerLocation?.Postcode);
        actual.Ukprn.Should().Be(source.TrainingProvider.Ukprn.ToString());
    }

}