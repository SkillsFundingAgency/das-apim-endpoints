using SFA.DAS.EmployerFinance.Models.Projections;
using System;

namespace SFA.DAS.EmployerFinance.UnitTests.Models.Projections;

[TestFixture]
internal class WhenGettingLevyCreationDateFromPayrollDate
{
    [TestCase("24-25", (short)1, 2024, 5)]
    [TestCase("24-25", (short)8, 2024, 12)]
    [TestCase("24-25", (short)9, 2025, 1)]
    [TestCase("24-25", (short)10, 2025, 2)]
    [TestCase("24-25", (short)11, 2025, 3)]
    [TestCase("24-25", (short)12, 2025, 4)]
    public void Then_The_Date_Is_The_Month_After_The_Payroll_Month(string payrollYear, short payrollMonth, int expectedYear, int expectedMonth)
    {
        var declaration = new LevyDeclaration { PayrollYear = payrollYear, PayrollMonth = payrollMonth };

        var result = declaration.GetLevyCreationDateFromPayrollDate();

        result.Should().Be(new DateTime(expectedYear, expectedMonth, 1));
    }

    [TestCase(null, (short)1)]
    [TestCase("", (short)1)]
    [TestCase("24-25", null)]
    public void Then_Null_Is_Returned_When_The_Payroll_Period_Is_Incomplete(string payrollYear, short? payrollMonth)
    {
        var declaration = new LevyDeclaration { PayrollYear = payrollYear, PayrollMonth = payrollMonth };

        var result = declaration.GetLevyCreationDateFromPayrollDate();

        result.Should().BeNull();
    }
}
