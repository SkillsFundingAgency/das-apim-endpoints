namespace SFA.DAS.EpaoRegister.InnerApi.Responses
{
    public class GetEpaosListItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint? Ukprn { get; set; }
        public string Status { get; set; }
        public int? OrganisationTypeId { get; set; }
    }
}