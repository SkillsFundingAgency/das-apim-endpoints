using System;

namespace SFA.DAS.EmployerAccounts.Api.Models
{
    public class OrganisationResponse
    {
        public string Name { get; set; }
        public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationType Type { get; set; }
        public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationSubType SubType { get; set; }
        public string Code { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public SharedOuterApi.InnerApi.Responses.ReferenceData.Address Address { get; set; }
        public string Sector { get; set; }
        public SharedOuterApi.InnerApi.Responses.ReferenceData.OrganisationStatus OrganisationStatus { get; set; }

        public static implicit operator OrganisationResponse(Application.Models.Organisation source)
        {
            if (source == null)
            {
                return null;
            }

            return new OrganisationResponse
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
