using Microsoft.AspNetCore.Http;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.Import
{
    internal class ImportPldnsApiRequest : IPostApiRequest<IFormFile>
    {
        public string PostUrl => "/api/import/pldns";

        public IFormFile Data { get; set; }
    }
}
