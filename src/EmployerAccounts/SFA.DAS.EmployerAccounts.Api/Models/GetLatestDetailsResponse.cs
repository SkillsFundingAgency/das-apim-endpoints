using System;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.ReferenceData;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class GetLatestDetailsResponse
    {
        public Organisation Organisation { get; set; }
        public static implicit operator GetLatestDetailsResponse(GetLatestDetailsResult source)
        {
            return new GetLatestDetailsResponse
            {
                Organisation = (Organisation)source.OrganisationDetail
            };
        }
    }

    public class Organisation
    {
        public string Name { get; set; }
        public OrganisationType Type { get; set; }
        public OrganisationSubType SubType { get; set; }
        public string Code { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public Address Address { get; set; }
        public string Sector { get; set; }
        public OrganisationStatus OrganisationStatus { get; set; }

        public static implicit operator Organisation(GetLatestDetailsResult.Organisation source)
        {
            if (source == null)
            {
                return null;
            }

            return new Organisation
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
    }
}
