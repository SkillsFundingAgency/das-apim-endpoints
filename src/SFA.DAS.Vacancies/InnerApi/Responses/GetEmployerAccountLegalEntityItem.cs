using System;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetEmployerAccountLegalEntityItem
    {
        // subset of properties taken from das-employerapprenticeshipsservice\src\SFA.DAS.EmployerAccounts.Api.Types\LegalEntity.cs
        public string Address { get; set; }
        public string Name { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }
    }
}