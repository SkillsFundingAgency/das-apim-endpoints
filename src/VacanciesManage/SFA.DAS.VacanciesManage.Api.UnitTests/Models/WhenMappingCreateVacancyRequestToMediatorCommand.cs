using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.VacanciesManage.Api.Extensions;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using System;
using CreateVacancyDisabilityConfident = SFA.DAS.VacanciesManage.Api.Models.CreateVacancyDisabilityConfident;
using WageType = SFA.DAS.VacanciesManage.Api.Models.WageType;

namespace SFA.DAS.VacanciesManage.Api.UnitTests.Models;

public class WhenMappingCreateVacancyRequestToMediatorCommand
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped_For_RequestV2(CreateVacancyRequest source)
    {
        // arrange
        source.RecruitingNationally = false;
        source.MultipleAddresses = null;
        source.Wage.WageType = WageType.FixedWage;

        // act
        var actual = (PostVacancyV2RequestData)source;

        // assert
        actual.Should().BeEquivalentTo(source, options => options
            .Excluding(c => c.StartDate)
            .Excluding(c => c.ClosingDate)
            .Excluding(c => c.Wage.WageType)
            .Excluding(c => c.Wage.DurationUnit)
            .Excluding(c => c.ContractingParties)
            .Excluding(c => c.Address)
            .Excluding(c => c.MultipleAddresses)
            .Excluding(c => c.RecruitingNationally)
            .Excluding(c => c.RecruitingNationallyDetails)
            .Excluding(c => c.ApplicationMethod)
            .Excluding(c => c.SubmitterContactDetails)
            .Excluding(c => c.EmployerNameOption)
            .Excluding(c => c.DisabilityConfident)
            .Excluding(c => c.Qualifications)
        );
        actual.StartDate.Should().BeCloseTo(source.StartDate, TimeSpan.FromSeconds(5));
        actual.ClosingDate.Should().BeCloseTo(source.ClosingDate, TimeSpan.FromSeconds(5));
        actual.Wage.WageType.Should().Be(source.Wage.WageType.ToDomainWageType());
        actual.Wage.DurationUnit.Should().Be(source.Wage.DurationUnit.ToDomainDurationUnit());
        actual.ApplicationMethod.Should().Be(source.ApplicationMethod.ToDomainApplicationMethod());
        actual.DisabilityConfident.Should().Be(source.DisabilityConfident == CreateVacancyDisabilityConfident.Yes);
        actual.Qualifications.Should().BeEquivalentTo(source.Qualifications, options => options
            .Excluding(c => c.Weighting)
        );
    }

    [Test]
    [InlineAutoData(WageType.NationalMinimumWage)]
    [InlineAutoData(WageType.NationalMinimumWageForApprentices)]
    public void Then_If_The_Wage_Is_Not_Fixed_Then_Value_Set_To_Null_For_V2(WageType wageType, CreateVacancyRequest source)
    {
        source.Wage.WageType = wageType;

        var actual = (PostVacancyV2RequestData)source;

        actual.Wage.FixedWageYearlyAmount.Should().BeNull();
    }

    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped_For_Single_Address_For_V2(CreateVacancyRequest source)
    {
        // arrange
        source.RecruitingNationally = false;
        source.RecruitingNationallyDetails = null;
        source.MultipleAddresses = null;

        // act
        var actual = (PostVacancyV2RequestData)source;

        // assert
        actual.EmployerLocations.Should().BeEquivalentTo([source.Address]);
        actual.EmployerLocationOption.Should().Be(Recruit.Contracts.ApiResponses.AvailableWhere.OneLocation);
        actual.EmployerLocationInformation.Should().BeNull();
    }

    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped_For_Multiple_Addresses_For_V2(CreateVacancyRequest source)
    {
        // arrange
        source.Address = null;
        source.RecruitingNationally = false;
        source.RecruitingNationallyDetails = null;

        // act
        var actual = (PostVacancyV2RequestData)source;

        // assert
        actual.EmployerLocations.Should().BeEquivalentTo(source.MultipleAddresses);
        actual.EmployerLocationOption.Should().Be(Recruit.Contracts.ApiResponses.AvailableWhere.MultipleLocations);
        actual.EmployerLocationInformation.Should().BeNull();
    }

    [Test, AutoData]
    public void Then_The_Fields_Are_Correctly_Mapped_For_Recruiting_Nationally_For_V2(CreateVacancyRequest source)
    {
        // arrange
        source.Address = null;
        source.MultipleAddresses = null;

        // act
        var actual = (PostVacancyV2RequestData)source;

        // assert
        actual.EmployerLocations.Should().BeNull();
        actual.EmployerLocationOption.Should().Be(Recruit.Contracts.ApiResponses.AvailableWhere.AcrossEngland);
    }
}