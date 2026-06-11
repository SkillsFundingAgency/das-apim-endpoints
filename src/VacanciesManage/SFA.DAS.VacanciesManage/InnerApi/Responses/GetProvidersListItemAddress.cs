namespace SFA.DAS.VacanciesManage.InnerApi.Responses;

public sealed record GetProvidersListItemAddress
{
    public string AddressLine1 { get; set; }
    public string AddressLine2 { get; set; }
    public string AddressLine3 { get; set; }
    public string AddressLine4 { get; set; }
    public string Town { get; set; }
    public string Postcode { get; set; }
}
