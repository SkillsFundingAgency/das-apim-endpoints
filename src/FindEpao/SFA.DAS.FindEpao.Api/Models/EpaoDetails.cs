using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class EpaoDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint? Ukprn { get; set; }
        public string Status { get; set; }
        public int? OrganisationTypeId { get; set; }
        public string PrimaryContactName { get; set; }
        public EpaoDetailsOrganisationData OrganisationData { get; set; }

        public static implicit operator EpaoDetails(GetEpaoResponse source)
        {
            return new EpaoDetails
            {
                Id = source.Id,
                Name = source.Name,
                Ukprn = source.Ukprn,
                Status = source.Status,
                OrganisationTypeId = source.OrganisationTypeId,
                PrimaryContactName = source.PrimaryContactName,
                OrganisationData = source.OrganisationData
            };
        }
    }
}