using SFA.DAS.EmployerDemand.InnerApi.Responses;

namespace SFA.DAS.EmployerDemand.Api.Models
{
    public class GetProviderContactDetails
    {
        public int Ukprn { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }

        public static implicit operator GetProviderContactDetails(GetProviderCourseInformation source)
        {
            if (source == null)
            {
                return null;
            }
            
            return new GetProviderContactDetails
            {
                Ukprn = source.Ukprn,
                Website = source.ContactUrl,
                EmailAddress = source.Email,
                PhoneNumber = source.Phone
            };
        }
    }
}