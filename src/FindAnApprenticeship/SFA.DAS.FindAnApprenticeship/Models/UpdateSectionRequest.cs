namespace SFA.DAS.FindAnApprenticeship.Models
{
    public class PatchApplicationRequest
    {
        public string Path { get; set; }
        public string Op { get; set; }
        public SectionStatus Value { get; set; }
    }
}