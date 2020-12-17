using System.Collections.Generic;
using SFA.DAS.FindEpao.InnerApi.Responses;

namespace SFA.DAS.FindEpao.Api.Models
{
    public class GetCourseEpaoResponse
    {
        public EpaoDetails Epao { get; set; }
        public GetCourseListItem Course { get; set; }
        public int CourseEpaosCount { get; set; }
        public IEnumerable<EpaoDeliveryArea> EpaoDeliveryAreas { get; set; }
        public IEnumerable<GetDeliveryAreaListItem> DeliveryAreas { get; set; }
    }

    public class EpaoDetails
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint? Ukprn { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public int? OrganisationTypeId { get; set; }
        public string PrimaryContact { get; set; }
        public string PrimaryContactName { get; set; }
        public EpaoDetailsOrganisationData OrganisationData { get; set; }

        public static implicit operator EpaoDetails(GetEpaoResponse source)
        {
            return new EpaoDetails
            {
                Id = source.Id,
                Name = source.Name,
                Ukprn = source.Ukprn,
                Email = source.Email,
                Status = source.Status,
                OrganisationTypeId = source.OrganisationTypeId,
                PrimaryContact = source.PrimaryContact,
                PrimaryContactName = source.PrimaryContactName,
                OrganisationData = source.OrganisationData
            };
        }
    }
}