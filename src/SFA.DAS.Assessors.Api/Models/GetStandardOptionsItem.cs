using SFA.DAS.Assessors.InnerApi.Responses;
using System.Collections.Generic;

namespace SFA.DAS.Assessors.Api.Models
{
    public class GetStandardOptionsItem
    {
        public string StandardUid { get; set; }
        public int LarsCode { get; set; }
        public string IfateReferenceNumber { get; set; }
        public IEnumerable<string> Options { get; set; }

        public static implicit operator GetStandardOptionsItem(GetStandardOptionsListItem source)
        {
            return new GetStandardOptionsItem
            {
                StandardUid = source.StandardUid,
                IfateReferenceNumber = source.IfateReferenceNumber,
                LarsCode = source.LarsCode,
                Options = source.Options
            };
        }
    }
}
