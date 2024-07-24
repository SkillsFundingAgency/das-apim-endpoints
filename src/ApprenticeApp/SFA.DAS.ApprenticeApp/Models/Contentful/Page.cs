using System;
using System.Collections.Generic;
using Contentful.Core.Models;

namespace SFA.DAS.ApprenticeApp.Models.Contentful
{
    public class Page : IEntity
    {
        public PageSystemProperties Sys { get; set; }
        public string Slug { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }
        public int Order { get; set; }

        public static explicit operator Page(Entry<dynamic> v)
        {
            throw new NotImplementedException();
        }
    }
}
