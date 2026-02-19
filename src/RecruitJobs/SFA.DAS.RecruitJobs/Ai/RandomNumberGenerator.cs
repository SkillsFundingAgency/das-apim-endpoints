namespace SFA.DAS.RecruitJobs.Ai;

public class RandomNumberGenerator : IRandomNumberGenerator
{
    private static readonly Random Generator = new();
    
    public double NextDouble()
    {
        return Generator.NextDouble();
    }
}