namespace SFA.DAS.Apprenticeships.Responses;

public class GetAcademicYearsResponse
{
#pragma warning disable CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
	public string AcademicYear { get; set; }
#pragma warning restore CS8618 // Non-nullable field is uninitialized. Consider declaring as nullable.
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public DateTime HardCloseDate { get; set; }
}