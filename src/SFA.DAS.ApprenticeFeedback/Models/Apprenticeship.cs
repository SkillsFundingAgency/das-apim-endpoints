//using SFA.DAS.ApprenticeFeedback.InnerApi.Responses;
//using System;

//namespace SFA.DAS.ApprenticeFeedback.Models
//{
//    public class Apprenticeship
//    {
//        public string StandardUId { get; set; }
//        public int LarsCode { get; set; }
//        public string StandardReference { get; set; }
//        public DateTime? StartDate { get; set; }
//        public DateTime? EndDate { get; set; }
//        public DateTime? LastFeedbackCompletedDate { get; set; }
//        public ApprenticeshipStatus Status { get; set; }

//        public static implicit operator Apprenticeship(ApprenticeFeedbackTarget source)
//        {
//            return new Apprenticeship
//            {
//                StartDate = source.StartDate,
//                EndDate = source.EndDate,
//                // Need to think about whether the feedback target stores the larscode / standarduid / standard ref for cacheing.
//                //LarsCode = source.LarsCode,
//                //StandardUId = source.StandardUId,
//                //StandardReference = source.StandardReference,
//                LastFeedbackCompletedDate = source.LastFeedbackCompletedDate
//                //Status = source.Status
//            };
//        }
//    }

//    public enum ApprenticeshipStatus
//    {
//        Unknown = 0,
//        Active = 1,
//        Passed = 2,
//        Withdrawn = 3,
//        Stopped = 4,
//        Paused = 5
//    }
//}
