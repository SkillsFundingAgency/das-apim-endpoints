using System;

namespace SFA.DAS.ApprenticeApp.Models
{
    public class Ksb
    {
        public KsbType Type { get; set; }
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Detail { get; set; }


    }

    public enum KsbType
    {
        Knowledge = 1,
        Skill = 2,
        Behaviour = 3,
    }

    public class ApprenticeKsbCollection
    {
        public ApprenticeKsb[] ApprenticeKsbs { get; set; }
    }

    public class ApprenticeKsb : Ksb
    {
        public ApprenticeKsbProgressData? Progress { get; set; }
    }
}