namespace SFA.DAS.Earnings.Application.ApprovedApprenticeships;

public class ApprovedApprenticeshipCsvRecord(long id, long ukprn, int academicYear, long uln)
{
    public long Id { get; set; } = id;
    public long Ukprn { get; set; } = ukprn;
    public int AcademicYear { get; set; } = academicYear;
    public long Uln { get; set; } = uln;
}