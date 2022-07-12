using SFA.DAS.SharedOuterApi.Interfaces;
using System;

namespace SFA.DAS.Assessors.InnerApi.Requests
{
    public class GetAllLearnersRequest : IGetApiRequest
    {
        private string _url;

        public string GetUrl { get { return _url; } }

        
        public int BatchNumber { get; set; }
        public int BatchSize { get; set; }


        public GetAllLearnersRequest(DateTime? sinceTime, int batchNumber, int batchSize)
        {
            string sinceTimeParam = string.Empty;
            if(null != sinceTime && sinceTime.HasValue)
            {
                sinceTimeParam = sinceTime.Value.ToString("O", System.Globalization.CultureInfo.InvariantCulture);
            }

            _url = $"api/learners?sinceTime={sinceTimeParam}&batch_number={batchNumber}&batch_size={batchSize}";
        }
    }
}
