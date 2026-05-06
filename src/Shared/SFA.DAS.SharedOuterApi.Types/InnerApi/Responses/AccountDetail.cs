namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses
{
    public class AccountDetail
    {
        public string PublicHashedAccountId { get; set; }
        public string DasAccountName { get; set; }
        public long AccountId { get; set; }
        public List<Resource> LegalEntities { get; set; }
    }
    public class Resource
    {
        public string Id { get; set; }
        public string Href { get; set; }
    }
}