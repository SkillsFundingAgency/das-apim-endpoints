using Microsoft.AspNetCore.Http;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Import;

public class ImportDefundingListApiRequest : IPostApiRequest<IFormFile>
{
    public string PostUrl => "/api/import/defunding-list";
    public IFormFile Data { get; set; }
}