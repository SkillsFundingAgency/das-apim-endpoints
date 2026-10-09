using System;

namespace SFA.DAS.EmployerFinance.Models.Projections;

public sealed record LevyDeclaration
{
    public string PayeSchemeReference { get; set; }
    public DateTime? SubmissionDate { get; set; }
    public string PayrollYear { get; set; }
    public short? PayrollMonth { get; set; }
    public DateTime CreatedDate { get; set; }
    public decimal TopUp { get; set; }
    public decimal TotalAmount { get; set; }

    public DateTime? PayrollDate()
    {
        if (PayrollMonth == null || string.IsNullOrEmpty(PayrollYear))
        {
            return null;
        }

        var year = 2000;
        var month = 1;

        if (PayrollMonth <= 9)
        {
            year += Convert.ToInt32(PayrollYear.Split('-')[0]);
            month = (short)(PayrollMonth + 3);
        }
        else
        {
            year += Convert.ToInt32(PayrollYear.Split('-')[1]);
            month = (short)(PayrollMonth - 9);
        }

        var dateTime = new DateTime(year, month, 1);

        return dateTime;
    }
}