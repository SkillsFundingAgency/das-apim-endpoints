using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class EpaoListItem
    {
        public string Id { get; set; }
        public uint Ukprn { get; set; }
        public string Name { get; set; }
        public IEnumerable<Link> Links => BuildLinks();

        public static implicit operator EpaoListItem(InnerApi.Responses.GetEpaosListItem source)
        {
            return new EpaoListItem
            {
                Id = source.Id,
                Ukprn = source.Ukprn,
                Name = source.Name
            };
        }

        private IEnumerable<Link> BuildLinks()
        {
            return new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = $"/epaos{Id}"
                },
                new Link
                {
                    Rel = "courses",
                    Href = $"/epaos{Id}/courses"
                }
            };
        }
    }
}