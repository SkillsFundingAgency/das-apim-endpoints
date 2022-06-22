using SFA.DAS.EpaoRegister.InnerApi.Responses;

namespace SFA.DAS.EpaoRegister.Api.Models
{
    public class EpaoAddress
    {
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Postcode { get; set; }

        public static explicit operator EpaoAddress(GetEpaoAddress source)
        {
            if (source == null)
                return null;

            return new EpaoAddress
            {
                Address1 = source.Address1,
                Address2 = source.Address2,
                Address3 = source.Address3,
                Address4 = source.Address4,
                Postcode = source.Postcode
            };
        }
    }
}