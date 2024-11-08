using System.Collections.Generic;
using System.Diagnostics.Metrics;




namespace SFA.DAS.ApprenticeApp.Telemetry
{
    public interface IApprenticeAppMetrics
    {
        void IncreaseAccountViews(int viewCount = 1);
        void IncreaseKSBsViews(string courseId, int viewCount = 1);
        void IncreaseKSBInProgress(string courseId, string ksbId, int viewCount = 1);
        void IncreaseKSBCompleted(string courseId, string ksbId, int viewCount = 1);
        void IncreaseSupportGuidanceArticleViews(string articleId, int viewCount = 1);
    }
    public class ApprenticeAppMetrics : IApprenticeAppMetrics
    {
        private Counter<long> AccountViewsCounter { get; }
        private Counter<long> KSBsViewsCounter { get; }
        private Counter<long> KSBInProgressCounter { get; }
        private Counter<long> KSBCompletedCounter { get; }
        private Counter<long> SupportAndGuidanceArticleViewsCounter { get; }
        public ApprenticeAppMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("MyApprenticeshipApp");
            AccountViewsCounter = meter.CreateCounter<long>("MyApprenticeship-App.accounts.views", "account", "Instrument used to measure the number of times the apprentice accounts is viewed");
            KSBsViewsCounter = meter.CreateCounter<long>("MyApprenticeship-App.ksb.views", "ksbs", "Instrument used to measure the number of times the course KSBs are viewed");
            KSBInProgressCounter = meter.CreateCounter<long>("MyApprenticeship-App.ksb.inprogress", "ksb", "Instrument used to measure the number of a KSB marked as in-progress");
            KSBCompletedCounter = meter.CreateCounter<long>("MyApprenticeship-App.ksb.completed", "ksb", "Instrument used to measure the number of times a KSB is marked as completed");
            SupportAndGuidanceArticleViewsCounter = meter.CreateCounter<long>("MyApprenticeship-App.supportguidancearticle.views", "article", "Instrument used to measure the number of support and guidance articles viewed");
        }
        public void IncreaseAccountViews(int viewCount = 1)
        {
            AccountViewsCounter.Add(viewCount);
        }
        public void IncreaseKSBsViews(string courseId, int viewCount = 1)
        {
            KSBsViewsCounter.Add(viewCount, new KeyValuePair<string, object>("course.id", courseId));
        }
        public void IncreaseKSBInProgress(string courseId, string ksbId, int viewCount = 1)
        {
            KSBInProgressCounter.Add(viewCount,
                new KeyValuePair<string, object>("course.id", courseId),
                new KeyValuePair<string, object>("ksb.id", ksbId));
        }
        public void IncreaseKSBCompleted(string courseId, string ksbId, int viewCount = 1)
        {
            KSBCompletedCounter.Add(viewCount,
                new KeyValuePair<string, object>("course.id", courseId),
                new KeyValuePair<string, object>("ksb.id", ksbId));
        }
        public void IncreaseSupportGuidanceArticleViews(string articleId, int viewCount = 1)
        {
            SupportAndGuidanceArticleViewsCounter.Add(viewCount,
                new KeyValuePair<string, object>("article.id", articleId));
        }
    }
}