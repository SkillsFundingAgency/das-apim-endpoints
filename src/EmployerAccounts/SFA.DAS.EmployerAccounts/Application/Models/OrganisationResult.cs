using System;
using SFA.DAS.EmployerAccounts.ExternalApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.EducationalOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.PublicSectorOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Application.Models;

public class OrganisationResult
{
    public string Name { get; set; }
    public OrganisationType Type { get; set; }
    public OrganisationSubType SubType { get; set; }
    public string Code { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public Address Address { get; set; }
    public string Sector { get; set; }
    public OrganisationStatus OrganisationStatus { get; set; }

    public static implicit operator OrganisationResult(Organisation source)
    {
        if (source == null)
        {
            return null;
        }
        return new OrganisationResult
        {
            Name = source.Name,
            Type = source.Type,
            SubType = source.SubType,
            Code = source.Code,
            RegistrationDate = source.RegistrationDate,
            Address = source.Address,
            Sector = source.Sector,
            OrganisationStatus = source.OrganisationStatus
        };
    }

    public static implicit operator OrganisationResult(EducationalOrganisation source)
    {
        if (source == null)
        {
            return null;
        }

        return new OrganisationResult
        {
            Name = source.Name,
            Type = OrganisationType.EducationOrganisation,
            SubType = OrganisationSubType.None,
            Code = source.URN,
            RegistrationDate = null,
            Address = new Address
            {
                Line1 = source.AddressLine1,
                Line2 = source.AddressLine2,
                Line3 = source.AddressLine3,
                Line4 = source.Town,
                Line5 = source.County,
                Postcode = source.PostCode
            },
            Sector = source.EducationalType
        };
    }

    public static implicit operator OrganisationResult(PublicSectorOrganisation source)
    {
        if (source == null)
        {
            return null;
        }

        return new OrganisationResult
        {
            Name = source.Name,
            Type = OrganisationType.PublicSector,
            SubType = MapToOrganisationSubType(source.Source),
            Code = source.Id.ToString(),
            RegistrationDate = null,
            Address = new Address
            {
                Line1 = source.AddressLine1,
                Line2 = source.AddressLine2,
                Line3 = source.AddressLine3,
                Line4 = source.Town,
                Line5 = source.Country,
                Postcode = source.PostCode
            },
            Sector = source.OnsSector,
            OrganisationStatus = source.Active ? OrganisationStatus.Active : OrganisationStatus.None
        };
    }

    public static implicit operator OrganisationResult(GetCompanyInfoResponse source)
    {
        if (source == null)
        {
            return null;
        }

        return new OrganisationResult
        {
            Name = source.CompanyName,
            Type = OrganisationType.Company,
            SubType = OrganisationSubType.None,
            Code = source.CompanyNumber,
            RegistrationDate = source.DateOfIncorporation,
            Address = new Address
            {
                Line1 = source.RegisteredAddress.Line1,
                Line2 = source.RegisteredAddress.Line2,
                Line3 = source.RegisteredAddress.Line3,
                Line4 = source.RegisteredAddress.TownOrCity,
                Line5 = source.RegisteredAddress.County,
                Postcode = source.RegisteredAddress.PostCode
            },
            Sector = null,
            OrganisationStatus = MapCompanyToOrganisationStatus(source.CompanyStatus)
        };
    }

    public static implicit operator OrganisationResult(CompanySearchResultsItem source)
    {
        if (source == null)
        {
            return null;
        }

        var address = source.Address != null ? new Address
        {
            Line1 = source.Address.Line1,
            Line2 = source.Address.Line2,
            Line3 = source.Address.Line3,
            Line4 = source.Address.TownOrCity,
            Line5 = source.Address.County,
            Postcode = source.Address.PostCode
        } : new Address();

        return new OrganisationResult
        {
            Name = source.CompanyName,
            Type = OrganisationType.Company,
            SubType = OrganisationSubType.None,
            Code = source.CompanyNumber,
            RegistrationDate = source.DateOfIncorporation,
            Address = address,
            Sector = null,
            OrganisationStatus = MapCompanyToOrganisationStatus(source.CompanyStatus)
        };
    }

    public static implicit operator OrganisationResult(GetCharityResponse source)
    {
        if (source == null)
        {
            return null;
        }

        return new OrganisationResult
        {
            Name = source.Name,
            Type = OrganisationType.Charity,
            SubType = OrganisationSubType.None,
            Code = source.RegistrationNumber.ToString(),
            RegistrationDate = source.RegistrationDate,
            Address = new Address
            {
                Line1 = source.AddressLine1,
                Line2 = source.AddressLine2,
                Line3 = source.AddressLine3,
                Line4 = source.AddressLine4,
                Line5 = source.AddressLine5,
                Postcode = source.Postcode
            },
            Sector = null,
            OrganisationStatus = OrganisationStatus.None
        };
    }

    public static OrganisationSubType MapToOrganisationSubType(string value)
    {
        return value switch
        {
            "Police" => OrganisationSubType.Police,
            "Nhs" => OrganisationSubType.Nhs,
            "Ons" => OrganisationSubType.Ons,
            _ => OrganisationSubType.None
        };
    }
    private static OrganisationStatus MapCompanyToOrganisationStatus(string companiesHouseStatus)
    {
        Enum.TryParse(companiesHouseStatus?.Replace("-", string.Empty), true, out OrganisationStatus organisationStatus);

        return organisationStatus;
    }
}
