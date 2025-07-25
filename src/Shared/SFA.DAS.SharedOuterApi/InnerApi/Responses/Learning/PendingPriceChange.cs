﻿using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Learning
{
    public class PendingPriceChange
	{
		public decimal? OriginalTrainingPrice { get; set; }
		public decimal? OriginalAssessmentPrice { get; set; }
		public decimal OriginalTotalPrice { get; set; }
		public decimal? PendingTrainingPrice { get; set; }
		public decimal? PendingAssessmentPrice { get; set; }
		public decimal PendingTotalPrice { get; set; }
		public DateTime EffectiveFrom { get; set; }
		public string Reason { get; set; }
		public long? Ukprn { get; set; }
        public long? AccountLegalEntityId { get; set; }
        public string FirstName { get; set; }
		public string LastName { get; set; }
		public DateTime? ProviderApprovedDate { get; set; }
		public DateTime? EmployerApprovedDate { get; set; }
        public string Initiator { get; set; }
    }
}