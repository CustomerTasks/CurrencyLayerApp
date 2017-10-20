using System;
using NUnit.Framework;

namespace CurrencyLayerApp.Tests
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestMethod()
        {
            try
            {
                var data = DateTime.Now;
                string str2 = data.AddDays(-1).ToString("yyyy-MM-dd");
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
