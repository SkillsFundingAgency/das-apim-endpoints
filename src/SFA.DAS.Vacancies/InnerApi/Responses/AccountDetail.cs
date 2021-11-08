using System.Collections.Generic;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class AccountDetail
    {
        public string PublicHashedAccountId { get; set; }
        public List<Resource> LegalEntities { get; set; }
    }
    public class Resource
    {
        public string Id { get; set; }

    }
}