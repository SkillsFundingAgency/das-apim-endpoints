namespace SFA.DAS.TrackProgress.Application.Models;

public record UkPRN
{
	public long Value { get; set; }

	public override string ToString() => $"UKPRN {Value}";

	public static UkPRN? Parse(string input)
	{
		if (!long.TryParse(input, out var value)) return null;
		if (value < 1) return null;
		return new() { Value = value };
	}
}
