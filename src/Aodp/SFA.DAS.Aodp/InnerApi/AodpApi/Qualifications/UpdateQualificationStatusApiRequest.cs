using SFA.DAS.Aodp.Application.Commands.Qualification;
using SFA.DAS.AODP.Application.Commands.Qualification;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AODP.Domain.Qualifications.Requests
{
    public class UpdateQualificationStatusApiRequest : IPostApiRequest
    {

        public UpdateQualificationStatusApiRequest(UpdateQualificationStatusCommand data)
        {
            Data = data;
        }

        public string PostUrl => $"api/qualifications/qualificationstatus";

        public object Data { get; set; }
    }
}