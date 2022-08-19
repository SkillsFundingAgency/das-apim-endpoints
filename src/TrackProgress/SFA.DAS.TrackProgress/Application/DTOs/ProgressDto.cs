namespace SFA.DAS.TrackProgress.Application.DTOs;

public class ProgressDto
{
    public ProgressDetails? Progress { get; set; }

    public class ProgressDetails
    {
        public List<Ksb>? Ksbs { get; set; }
    }

    public class Ksb
    {
        public string? Id { get; set; }
        public int? Value { get; set; }
    }
}