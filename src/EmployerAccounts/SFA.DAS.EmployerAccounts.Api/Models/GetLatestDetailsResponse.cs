using System;
using SFA.DAS.EmployerAccounts.Application.Queries.GetLatestDetails;

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
        public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationType Type { get; set; }
        public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationSubType SubType { get; set; }
        public string Code { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public SharedOuterApi.InnerApi.Responses.ReferenceData.Address Address { get; set; }
        public string Sector { get; set; }
        public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationStatus OrganisationStatus { get; set; }

        public static implicit operator Organisation(Application.Models.Organisation source)
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
