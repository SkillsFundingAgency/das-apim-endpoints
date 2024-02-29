namespace SFA.DAS.EarlyConnect.InnerApi.Requests
{
    public class StudentFeedbackList
    {
        public IEnumerable<StudentFeedback> ListOfStudentFeedback { get; set; }
    }

    public class StudentFeedback
    {
        public Guid SurveyId { get; set; }
        public string StatusUpdate { get; set; }
        public string Notes { get; set; }
        public string UpdatedBy { get; set; }
        public int LogId { get; set; }
    }
}
