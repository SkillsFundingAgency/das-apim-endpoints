using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.EmployerFeedback.Configuration
{
    [ExcludeFromCodeCoverage]
    public class EmployerFeedbackConfiguration
    {
        public string ApimEndpointsRedisConnectionString { get; set; }

        public int AccountProvidersCourseStatusCompletionLag { get; set; }

        public int AccountProvidersCourseStatusStartLag { get; set; }

        public int AccountProvidersCourseStatusNewStartWindow { get; set; }
    }
}
