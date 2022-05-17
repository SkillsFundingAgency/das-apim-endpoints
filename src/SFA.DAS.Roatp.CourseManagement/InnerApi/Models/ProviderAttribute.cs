namespace SFA.DAS.Roatp.CourseManagement.InnerApi.Models
{
    public class ProviderAttribute
    {
        public string Name { get; set; }
        public int Strengths { get; set; }
        public int Weaknesses { get; set; }

        public ProviderAttribute(string name, int strengthCount, int weaknessCount)
        {
            Name = name;
            Strengths = strengthCount;
            Weaknesses = weaknessCount;
        }
    }
}