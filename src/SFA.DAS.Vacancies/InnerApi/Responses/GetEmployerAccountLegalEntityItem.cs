using System;

namespace SFA.DAS.Vacancies.InnerApi.Responses
{
    public class GetEmployerAccountLegalEntityItem
    {
        // todo: confirm properties list. taken from das-employerapprenticeshipsservice\src\SFA.DAS.EmployerAccounts.Api.Types\LegalEntity.cs
        //public List<Agreement> Agreements { get; set; }
        public string Code { get; set; }
        public string DasAccountId { get; set; }
        public DateTime? DateOfInception { get; set; }
        public string Address { get; set; }
        public long LegalEntityId { get; set; }
        public string Name { get; set; }
        public string PublicSectorDataSource { get; set; }
        public string Sector { get; set; }
        public string Source { get; set; }
        public short SourceNumeric { get; set; }
        public string Status { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string AccountLegalEntityPublicHashedId { get; set; }

        /*[Obsolete]
        public string AgreementSignedByName { get; set; }

        [Obsolete]
        public DateTime? AgreementSignedDate { get; set; }

        [Obsolete]
        public EmployerAgreementStatus AgreementStatus { get; set; }*/
    }
}