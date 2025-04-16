namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class Agreement
{
    public long Id { get; set; }
    public DateTime? SignedDate { get; set; }
    public string SignedByName { get; set; } = "";
    public int Status { get; set; }
    public int TemplateVersionNumber { get; set; }
    public int AgreementType { get; set; }
}
