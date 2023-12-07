using SFA.DAS.EarlyConnect.InnerApi.Responses;

namespace SFA.DAS.EarlyConnect.Api.Models
{
    public class GetLEPSDataWithUsersResponse
    {
        public int Id { get; set; }
        public string LepCode { get; set; }
        public string Region { get; set; }
        public string LepName { get; set; }
        public string EntityEmail { get; set; }
        public string Postcode { get; set; }
        public DateTime DateAdded { get; set; }
        public ICollection<LEPSUserResponse>? Users { get; set; }
        public static implicit operator GetLEPSDataWithUsersResponse(LEPSData source)
        {
            return new GetLEPSDataWithUsersResponse
            {
                Id = source.Id,
                LepCode = source.LepCode,
                Region = source.Region,
                LepName = source.LepName,
                EntityEmail = source.EntityEmail,
                Postcode = source.Postcode,
                DateAdded = source.DateAdded,
                Users = source.LEPSUsers.Select(c => (LEPSUserResponse)c).ToList()
            };
        }
    }

    public class LEPSUserResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime DateAdded { get; set; }
        public static implicit operator LEPSUserResponse(LEPSUser source)
        {
            return new LEPSUserResponse
            {
                Id = source.Id,
                FirstName = source.FirstName,
                LastName = source.LastName,
                Email = source.Email,
                DateAdded = source.DateAdded
            };
        }
    }
}

