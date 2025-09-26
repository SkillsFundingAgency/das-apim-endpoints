using SFA.DAS.Aodp.Application.Commands.Qualification;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.Qualifications.Requests
{
    public class AddQualificationDiscussionHistoryApiRequest : IPostApiRequest
    {

        public AddQualificationDiscussionHistoryApiRequest(AddQualificationDiscussionHistoryCommand data)
        {
            Data = data;
        }

        public string PostUrl => $"api/qualifications/qualificationdiscussionhistory";

        public object Data { get; set; }
    }
}