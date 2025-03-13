﻿namespace SFA.DAS.Aodp.Application.Queries.Qualifications
{
    public class GetNewQualificationsApiResponse
    {
        public int TotalRecords { get; set; }
        public int? Skip { get; set; }
        public int Take { get; set; }       
        public List<NewQualification> Data { get; set; } = new();       
    }   
}
