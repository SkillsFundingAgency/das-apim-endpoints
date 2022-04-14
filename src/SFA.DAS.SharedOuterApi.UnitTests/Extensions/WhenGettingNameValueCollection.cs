using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Extensions;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace SFA.DAS.SharedOuterApi.UnitTests.Extensions
{
    public class WhenGettingNameValueCollection
    {
        private static readonly object[] _stringLists =
            {
                new object[] {new List<string> { "vOne" } },
                new object[] {new List<string> { "vOne", "vTwo", "vThree" }}
            };

        [TestCaseSource("_stringLists")]
        public void Then_Returns_NameValueCollection_With_Same_Key(List<string> valueOne)
        {
            string key = "Key";
            var collection = valueOne.ToNameValueCollection(key);

            Assert.IsInstanceOf<NameValueCollection>(collection);
            for (int i = 0; i < collection.Count; i++)
            {
                Assert.IsTrue(collection.GetKey(i) == key);
            }

        }
    }
}