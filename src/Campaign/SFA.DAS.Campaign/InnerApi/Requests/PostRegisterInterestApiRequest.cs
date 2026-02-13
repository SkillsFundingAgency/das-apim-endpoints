using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.InnerApi.Requests;

public class PostRegisterInterestApiRequest(EnquiryUserDataModel data) : IPostApiRequest
{
    public object Data { get; set; } = data;
    public string PostUrl => "api/registercampaigninterest/registerinterest";
}
