using System.Collections.Generic;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class AccountDetail
    {
        public List<Resource> LegalEntities { get; set; }
    }
    public class Resource
    {
        public string Id { get; set; }

    }
}