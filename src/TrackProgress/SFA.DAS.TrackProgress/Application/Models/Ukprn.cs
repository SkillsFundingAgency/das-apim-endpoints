namespace SFA.DAS.TrackProgress.Application.Models;

public record UkPrn
{
	public static long Parse(string input)
	{		
		if (!long.TryParse(input, out var value)) return 0;
		if (value < 1) return 0;
		return value;
	}
}
