using System.Collections.Generic;
using System.Linq;
using SFA.DAS.Vacancies.Manage.Application.Qualifications.Queries.GetQualifications;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;

namespace SFA.DAS.Vacancies.Manage.Api.Models
{
    public class GetQualificationsResponse
    {
        public List<GetQualificationsItem> Qualifications { get ; set ; }

        public static implicit operator GetQualificationsResponse(
            GetQualificationsQueryResponse source)
        {
            if (source.Qualifications == null)
            {
                return new GetQualificationsResponse
                {
                    Qualifications = new List<GetQualificationsItem>()
                };
            }
            
            return new GetQualificationsResponse
            {
                Qualifications = source.Qualifications.Select(c=>(GetQualificationsItem)c).ToList()
            };
        }
    }

    public class GetQualificationsListItem
    {
        public string GcseOrEquivalent { get; set; }
        public string AsLevelOrEqivalant { get; set; }
        public string ALevelOrEquivalent { get; set; }
        public string BtecOrEquivalent { get; set; }
        public string NvqOrSvqLevel1OrEquivalent { get; set; }
        public string NvqOrSvqLevel2OrEquivalent { get; set; }
        public long NvqOrSvqLevel3OrEquivalent { get; set; }
        public long Other { get; set; }



        public static implicit operator GetQualificationsListItem(GetQualificationsItem source)
        {
            return new GetQualificationsListItem
            {
                GcseOrEquivalent = source.GcseOrEquivalent,
                AsLevelOrEqivalant = source.AsLevelOrEqivalant,
                ALevelOrEquivalent = source.ALevelOrEquivalent,
                BtecOrEquivalent = source.BtecOrEquivalent,
                NvqOrSvqLevel1OrEquivalent = source.NvqOrSvqLevel1OrEquivalent,
                NvqOrSvqLevel2OrEquivalent = source.NvqOrSvqLevel2OrEquivalent,
                NvqOrSvqLevel3OrEquivalent = source.NvqOrSvqLevel3OrEquivalent,
                Other = source.Other,
            };
        }
    }
}