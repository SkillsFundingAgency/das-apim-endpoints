using System;
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
}