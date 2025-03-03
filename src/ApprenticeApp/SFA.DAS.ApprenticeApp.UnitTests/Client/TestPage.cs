using SFA.DAS.ApprenticeApp.Models.Contentful;
using System.Collections.Generic;

namespace SFA.DAS.ApprenticeApp.UnitTests.Client
{
    public class TestPage : IEntity
    {
        public List<IEntity> Content { get; set; }
        public PageSystemProperties Sys { get; set; }
    }
}