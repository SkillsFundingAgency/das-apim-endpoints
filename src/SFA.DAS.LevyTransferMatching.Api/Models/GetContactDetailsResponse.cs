using SFA.DAS.LevyTransferMatching.Application.Queries.GetContactDetails;
using System.Collections.Generic;
using System.Linq;

namespace SFA.DAS.LevyTransferMatching.Api.Models
{
    public class GetContactDetailsResponse
    {
        public int AllSectorsCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeSectors { get; set; }
        public int AllJobRolesCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeJobRoles { get; set; }
        public int AllLevelsCount { get; set; }
        public IEnumerable<ReferenceDataItem> PledgeLevels { get; set; }
        public int PledgeAmount { get; set; }
        public string PledgeDasAccountName { get; set; }

        public static implicit operator GetContactDetailsResponse(GetContactDetailsResult getContactDetailsResult)
        {
            var pledgeJobRoles = getContactDetailsResult.PledgeJobRoles
                .Select(x => new ReferenceDataItem()
                {
                    Id = x.Id,
                    Description = x.Description,
                });

            // Note: levels here are different - the ShortDescription is used
            var pledgeLevels = getContactDetailsResult.PledgeLevels
                .Select(x => new ReferenceDataItem()
                {
                    Id = x.Id,
                    Description = x.ShortDescription,
                });

            var pledgeSectors = getContactDetailsResult.PledgeSectors
                .Select(x => new ReferenceDataItem()
                {
                    Id = x.Id,
                    Description = x.Description,
                });

            return new GetContactDetailsResponse()
            {
                AllJobRolesCount = getContactDetailsResult.AllJobRolesCount,
                AllLevelsCount = getContactDetailsResult.AllLevelsCount,
                AllSectorsCount = getContactDetailsResult.AllSectorsCount,
                PledgeAmount = getContactDetailsResult.PledgeAmount,
                PledgeDasAccountName = getContactDetailsResult.PledgeDasAccountName,
                PledgeJobRoles = pledgeJobRoles,
                PledgeLevels = pledgeLevels,
                PledgeSectors = pledgeSectors,
            };
        }

        public class ReferenceDataItem
        {
            public string Id { get; set; }
            public string Description { get; set; }
        }
    }
}