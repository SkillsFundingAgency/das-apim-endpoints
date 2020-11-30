using SFA.DAS.EpaoRegister.InnerApi.Responses;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class GetEpaoApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint Ukprn { get; set; }
        public string Email { get; set; }
        public EpaoAddress Address { get; set; }
        public IEnumerable<Link> Links { get; set; }

        public static explicit operator GetEpaoApiModel(SearchEpaosListItem source)
        {
            if (source == null)
                return null;

            return new GetEpaoApiModel
            {
                Id = source.Id,
                Name = source.Name,
                Ukprn = source.Ukprn,
                Email = source.Email,
                Address = (EpaoAddress)source.Address
            };
        }
    }

    public class EpaoAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string Postcode { get; set; }

        public static explicit operator EpaoAddress(SearchEpaosAddress source)
        {
            if (source == null)
                return null;

            return new EpaoAddress
            {
                Address1 = source.Address1,
                Address2 = source.Address2,
                Address3 = source.Address3,
                City = source.City,
                Postcode = source.Postcode
            };
        }
    }
}