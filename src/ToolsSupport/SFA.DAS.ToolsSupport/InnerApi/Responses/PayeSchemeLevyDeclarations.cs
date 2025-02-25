namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class PayeSchemeLevyDeclarations
{
    public string Id { get; set; } = "";
    public string EmpRef { get; set; } = "";

    public List<Declaration> Declarations { get; set; } = [];
}

public class Declaration
{
    public long Id { get; set; }
    public long DeclarationId { get; set; }
    public DateTime SubmissionTime { get; set; }

    public PayrollPeriod PayrollPeriod { get; set; } = new();

    public decimal LevyDueYTD { get; set; }

    public decimal LevyAllowanceForFullYear { get; set; }

}
public class PayrollPeriod
{
    public string Year { get; set; } = "";

    public short Month { get; set; }
}